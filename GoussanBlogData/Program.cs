using Azure.Identity;
using GoussanBlogData.Services;
using Goussanjarga.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Azure Key Vault
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Populate Configuration File at RunTime with Data from Key Vault
var Configuration = builder.Configuration;
Config.AzureStorageConnectionString = Configuration["GoussanStorage"];
Config.AzureCosmosConnectionString = Configuration["GoussanCosmos"];
Config.CosmosDBName = Configuration["CosmosDb:DatabaseName"];
Config.AzureStorageBlob = Configuration["GoussanStorage:blob"];
Config.AzureStorageQueue = Configuration["GoussanStorage:queue"];
Config.AzureAppInsight = Configuration["AppInsightConString"];
Config.CosmosVideos = Configuration["CosmosDb:Containers:Videos:containerName"];
Config.AadClientId = Configuration["AadClientId"];
Config.AadSecret = Configuration["AadSecret"];
Config.AadTenantDomain = Configuration["AzureAd:AadTenantDomain"];
Config.AadTenantId = Configuration["AadTenantId"];
Config.AccountName = Configuration["AADAccountName"];
Config.ResourceGroup = Configuration["AADResourceGroup"];
Config.SubscriptionId = Configuration["AADSubscriptionId"];
Config.ArmAadAudience = Configuration["AzureAd:ArmAadAudience"];
Config.ArmEndpoint = Configuration["AzureAd:ArmEndpoint"];
Config.AppName = "GoussanMedia";
Config.AppRegion = Regions.WestEurope;

// Add services to the container.
builder.Services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync());
builder.Services.AddControllers();
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

app.MapControllers();

app.Run();


CosmosDbService InitializeCosmosClientInstanceAsync()
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
    string containerName = Configuration.GetSection("CosmosDb")["Container"];
    // Initialize the client
    CosmosDbService cosmosDbService = new(cosmosClient, databaseName,containerName);
    
    return cosmosDbService;
}