using Azure.Identity;
using GoussanBlogData.Models;
using GoussanBlogData.Services;
using GoussanBlogData.Services.Data;
using GoussanBlogData.Utils;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Management.Media;
using Microsoft.Identity.Client;
using Microsoft.Rest;

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
Config.AzureCosmosConnectionString = Configuration["GoussanCosmos"]; // Cosmos DB Connection String
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

// Add services to the container.
builder.Services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync().GetAwaiter().GetResult());
builder.Services.AddSingleton<IGoussanMediaService>(InitializeMediaService().GetAwaiter().GetResult());
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GoussanBlogData", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoussanBlogData v1"));
}
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
            await databaseClient.CreateContainerIfNotExistsAsync(containerName, paritionKeyPath, throughput: 400);
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
    _ = await azMediaService.EnsureStreamingEndpoint();

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