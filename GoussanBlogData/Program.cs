using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using GoussanBlogData.Models;
using GoussanBlogData.Services;
using GoussanBlogData.Services.Data;
using GoussanBlogData.Utils;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Management.Media;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Models;
using Microsoft.Rest;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Azure Key Vault
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Populate Configuration File at RunTime with Data from Key Vault

var Configuration = builder.Configuration;
// Misc configuration to define Application Name and region from where it's set to run / create instances based off
Config.AppName = "GoussanMedia";
Config.AppRegion = Regions.WestEurope;
Config.Secret = Configuration["GoussanBlogSecret"]; // JWT Master Key Secret
Config.AzureCosmosConnectionString = Configuration["GoussanCosmosServerless"]; // Cosmos DB Connection String
Config.CosmosDBName = Configuration["CosmosDb:DatabaseName"]; // Cosmos DB database name
Config.CosmosUser = Configuration["CosmosDb:Containers:User:containerName"]; // Cosmos DB container name
Config.CosmosMedia = Configuration["CosmosDb:Containers:Media:containerName"]; // Cosmos DB container name
// Azure Active Directory Information / Configuration needed to use Azure Media Services and to create a service Client
Config.AadClientId = Configuration["AadClientId"];
Config.AadSecret = Configuration["AadSecret"];
Config.AadTenantDomain = Configuration["AzureAd:AadTenantDomain"];
Config.AadTenantId = Configuration["AadTenantId"];
Config.AccountName = Configuration["AADAccountName"];
Config.ResourceGroup = Configuration["AADResourceGroup"];
Config.SubscriptionId = Configuration["AADSubscriptionId"];
Config.ArmAadAudience = Configuration["AzureAd:ArmAadAudience"];
Config.ArmEndpoint = Configuration["AzureAd:ArmEndpoint"];
// Storage Connection string, also split into Blob connection and Queue connection string if needed
Config.AzureStorageConnectionString = Configuration["GoussanStorage"];
Config.AzureStorageBlob = Configuration["GoussanStorage:blob"];
Config.AzureStorageQueue = Configuration["GoussanStorage:queue"];
// Azure Application Insight for diagnostics, metrics and logs of the application
Config.AzureAppInsight = Configuration["AppInsightConString"];

// Add Singleton Services of the Azure Service Client instances since they are Thread-safe and this is recommended usage
builder.Services.AddSingleton<ICosmosDbService>(await InitializeCosmosClientInstanceAsync());
builder.Services.AddSingleton<IGoussanMediaService>(await InitializeMediaService());
builder.Services.AddSingleton<IBlobStorageService>(InitializeStorageClientInstance());
// Add Misc services for App to functions and for utilities to be initialized properly
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddApplicationInsightsTelemetry(Configuration);
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new() { Title = "GoussanBlogData", Version = "v1" });
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Goussanjarga Blog And Media API",
        Description = "A simple API that is used to communicate with different Azure Services and provide data to Client",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Alf Ragnar",
            Email = "alf@goussanjarga.com",
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license"),
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}
// Configure Swagger
app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoussanBlogData v1");
    c.RoutePrefix = string.Empty;
});
// Configure Cors, most of Endpoint interaction with the outside will be handled through Azure API Management,
// but otherwise it is handled by JWT token verification by the application
app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});
app.UseHttpsRedirection();
app.UseAuthorization();
// Add Jwt Token Middleware
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();
app.Run();


async Task<CosmosDbService> InitializeCosmosClientInstanceAsync()
{
    // Define Azure Cosmos Db Client options like preferred operation region and Application Name
    CosmosClientOptions options = new()
    {
        ApplicationName = Config.AppName,
        ApplicationRegion = Config.AppRegion
    };
    // Create the new Cosmos Database Client
    CosmosClient cosmosClient = new(Config.AzureCosmosConnectionString, options);
    // Get the predefined Database name from Config
    string databaseName = Config.CosmosDBName;
    // Check if database exists
    await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
    // Initialize the client
    CosmosDbService cosmosDbService = new(cosmosClient, databaseName);
    // Get DB client
    var databaseClient = cosmosClient.GetDatabase(databaseName);
    // Check if necessary containers exist that is used for write operation at runtime, creates the containers if they don't exist
    IEnumerable<IConfiguration> containerList = Configuration.GetSection("CosmosDb").GetSection("Containers").GetChildren();
    foreach (var item in containerList)
    {
        try
        {
            string containerName = item.GetSection("containerName").Value;
            string paritionKeyPath = item.GetSection("paritionKeyPath").Value;
            await databaseClient.CreateContainerIfNotExistsAsync(containerName, paritionKeyPath);
        }
        catch (CosmosException)
        {

            throw;
        }
    }
    return cosmosDbService;
}


async Task<GoussanMediaService> InitializeMediaService()
{
    // Create the new Azure Media Service Client
    ServiceClientCredentials serviceClientCredentials = await GetCredentialsAsync();
    AzureMediaServicesClient azureMediaServicesClient = new(serviceClientCredentials) { SubscriptionId = Config.SubscriptionId };
    // Create new Goussan Media Service instance using the service client created
    GoussanMediaService azMediaService = new(azureMediaServicesClient);

    // Ensure streaming endpoint is online ( Check if Endpoint is online or starts the endpoint if offline )
    var streamingEndpoint = await azMediaService.EnsureStreamingEndpoint();
    Config.StreamingEndpoint = streamingEndpoint.HostName;

    // One time Task to ensure that I have the desired encoding available ( Checks if encoding task is setup or creates it if not )
    _ = await azMediaService.GetOrCreateTransformAsync();

    return azMediaService;
}


static async Task<ServiceClientCredentials> GetCredentialsAsync()
{
    // Use ConfidentialClientApplicationBuilder.AcquireTokenForClient to get a token using a service principal with symmetric key
    string TokenType = "Bearer";
    var scopes = new[] { Config.ArmAadAudience + "/.default" };

    var app = ConfidentialClientApplicationBuilder.Create(Config.AadClientId)
        .WithClientSecret(Config.AadSecret)
        .WithAuthority(AzureCloudInstance.AzurePublic, Config.AadTenantId)
        .Build();
    var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync().ConfigureAwait(false);

    var token = new TokenCredentials(authResult.AccessToken, TokenType);
    return token;
}


BlobStorageService InitializeStorageClientInstance()
{
    // Create the new Blob Service Client
    BlobServiceClient blobService = new(Config.AzureStorageConnectionString);
    // Get the predefined Container name from Config
    string container = Config.CosmosMedia;
    try
    {
        // Try to create blob container, will fail if container already exist
        blobService.GetBlobContainerClient(container);
    }
    catch (RequestFailedException)
    {
        // Throw error
        throw;
    }
    // Initialize the client
    BlobStorageService storageService = new(blobService, container);
    return storageService;
}
