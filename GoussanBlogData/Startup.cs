using Azure;
using Azure.Storage.Blobs;
using GoussanBlogData.Services;
using GoussanBlogData.Services.Data;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Management.Media;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Models;
using Microsoft.Rest;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace GoussanBlogData
{
    /// <summary>
    /// Starup class to configure the services and environment
    /// </summary>
    public class Startup
    {
        private readonly IWebHostEnvironment environment;
        private readonly ILogger<Startup> logger;

        /// <summary>
        /// Constructor to initialize the environment
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
        }

        /// <summary>
        /// Configuration property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public async void ConfigureServices(IServiceCollection services)
        {
            // SINGLETON SERVICES ( API TALKS TO AZURE )
            services.AddSingleton<ICosmosDbService>(await InitializeCosmosClientInstanceAsync(Configuration));
            services.AddSingleton<IGoussanMediaService>(await InitializeMediaService(Configuration));
            services.AddSingleton<IBlobStorageService>(await InitializeStorageClientInstance(Configuration));
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddApplicationInsightsTelemetry(Configuration.GetValue<string>("AzureAppInsight"));
            services.AddControllers();
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials();
                });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Goussanjarga Blog And Media API",
                    Description = "A simple API that is used to communicate with different Azure Services and provide data to Client. Use at own risk, there is no SLA.",
                    Contact = new OpenApiContact
                    {
                        Name = "Alf Ragnar",
                        Email = "alfi427@gmail.com",
                    },
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Function to initialize the Cosmos Client
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfiguration configuration)
        {
            var options = new CosmosClientOptions()
            {
                ApplicationName = configuration.GetSection("CosmosDb").GetValue<string>("AppName"),
                ApplicationRegion = configuration.GetSection("CosmosDb").GetValue<string>("Region")
            };

            var cosmosClient = new CosmosClient(configuration.GetConnectionString("CosmosDb"), options);
            var databaseName = configuration.GetSection("CosmosDb").GetValue<string>("DatabaseName");
            var res = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            if (res.StatusCode == HttpStatusCode.Created)
            {
                var databaseClient = cosmosClient.GetDatabase(databaseName);
                var containerList = configuration.GetSection("CosmosDb").GetSection("Containers").GetChildren();
                foreach (var item in containerList)
                {
                    try
                    {
                        var containerName = item.GetSection("containerName").Value;
                        var paritionKeyPath = item.GetSection("paritionKeyPath").Value;
                        await databaseClient.CreateContainerIfNotExistsAsync(containerName, paritionKeyPath);
                    }
                    catch (CosmosException ex)
                    {
                        logger.LogError(nameof(InitializeCosmosClientInstanceAsync), ex.Message);
                        throw new Exception("Cosmos DB Container Creation Failed");
                    }
                }
            }
            return new CosmosDbService(cosmosClient, databaseName);
        }

        /// <summary>
        /// Function to initialize the Azure Media Services Client
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private async Task<GoussanMediaService> InitializeMediaService(IConfiguration configuration)
        {
            var serviceClientCredentials = await GetServiceClientCredentialsAsync(configuration);
            var azureMediaServicesClient = new AzureMediaServicesClient(serviceClientCredentials)
            {
                SubscriptionId = configuration.GetValue<string>("SubscriptionID")
            };

            var azMediaService = new GoussanMediaService(azureMediaServicesClient);

            _ = await azMediaService.EnsureStreamingEndpoint();
            _ = await azMediaService.GetOrCreateTransformAsync();

            return azMediaService;
        }

        /// <summary>
        /// Used to get Auth token from Azure AD and return the Token Credentials
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<ServiceClientCredentials> GetServiceClientCredentialsAsync(IConfiguration configuration)
        {
            try
            {
                // Use ConfidentialClientApplicationBuilder.AcquireTokenForClient to get a token using a service principal with symmetric key
                string TokenType = "Bearer";
                var scopes = new[] { configuration.GetValue<string>("ArmAadAudience") + "/.default" };
                var clientId = configuration.GetValue<string>("AadClientId");
                var aadSecret = configuration.GetValue<string>("AadSecret");
                var aadTenantId = configuration.GetValue<string>("AadTenantId");

                var app = ConfidentialClientApplicationBuilder.Create(clientId)
                    .WithClientSecret(aadSecret)
                    .WithAuthority(AzureCloudInstance.AzurePublic, aadTenantId)
                    .Build();
                var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();

                var token = new TokenCredentials(authResult.AccessToken, TokenType);
                return token;
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(GetServiceClientCredentialsAsync), ex.Message);
                throw new Exception("Failed getting Service Client Credentials");
            }
        }

        /// <summary>
        /// Used to initialize Blob Service Client
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<BlobStorageService> InitializeStorageClientInstance(IConfiguration configuration)
        {
            var blobService = new BlobServiceClient(configuration.GetConnectionString("AzureStorageService"));
            var containers = configuration.GetSection("containers").GetChildren();
            try
            {
                var blobContainers = blobService.GetBlobContainers();

                foreach (var item in containers.Where(x =>
                        blobContainers.Any(blobContainer =>
                            blobContainer.Name.Equals(x.Value, StringComparison.InvariantCultureIgnoreCase))))
                {
                    await blobService.CreateBlobContainerAsync(item.Value.ToLower());
                }
            }
            catch (RequestFailedException ex)
            {
                logger.LogError(nameof(InitializeStorageClientInstance), ex.Message);
                throw new Exception("Failed initializing Storage Client");
            }
            var mediaContainer = containers.FirstOrDefault(x => x.Key.Equals("MediaContainer", StringComparison.InvariantCultureIgnoreCase));
            if (mediaContainer == null) throw new Exception("No Media Container in configuration");
            var storageService = new BlobStorageService(blobService, mediaContainer.Value);
            return storageService;
        }
    }
}