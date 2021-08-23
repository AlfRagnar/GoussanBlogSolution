using Azure.Identity;
using GoussanBlogData.Models;
using GoussanBlogData.Services;
using GoussanBlogData.Utils;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Configure Azure Key Vault
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Populate Configuration File at RunTime with Data from Key Vault
var Configuration = builder.Configuration;
Config.AppName = "GoussanMedia";
Config.AppRegion = Regions.WestEurope;
Config.Secret = Configuration["GoussanBlogSecret"];
Config.AzureCosmosConnectionString = Configuration["GoussanCosmos"];
Config.CosmosDBName = Configuration["CosmosDb:DatabaseName"];
Config.CosmosUser = Configuration["CosmosDb:Containers:User:containerName"];
Config.CosmosMedia = Configuration["CosmosDb:Containers:Media:containerName"];
//Config.AadClientId = Configuration["AadClientId"];
//Config.AadSecret = Configuration["AadSecret"];
//Config.AadTenantDomain = Configuration["AzureAd:AadTenantDomain"];
//Config.AadTenantId = Configuration["AadTenantId"];
//Config.AccountName = Configuration["AADAccountName"];
//Config.ResourceGroup = Configuration["AADResourceGroup"];
//Config.SubscriptionId = Configuration["AADSubscriptionId"];
//Config.ArmAadAudience = Configuration["AzureAd:ArmAadAudience"];
//Config.ArmEndpoint = Configuration["AzureAd:ArmEndpoint"];
//Config.AzureStorageConnectionString = Configuration["GoussanStorage"];
//Config.AzureStorageBlob = Configuration["GoussanStorage:blob"];
//Config.AzureStorageQueue = Configuration["GoussanStorage:queue"];
//Config.AzureAppInsight = Configuration["AppInsightConString"];

// Add services to the container.
builder.Services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync().GetAwaiter().GetResult());
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();
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
    // Create necessary containers to store META data in
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