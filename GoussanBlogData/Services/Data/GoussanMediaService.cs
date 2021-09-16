using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GoussanBlogData.Models;
using GoussanBlogData.Models.DatabaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoussanBlogData.Services.Data
{
    /// <summary>
    /// Handles Communication and interactions with Azure Media Services
    /// </summary>
    public class GoussanMediaService : IGoussanMediaService
    {
        private readonly AzureMediaServicesClient _azMediaServices;
        private readonly string resourceGroupName = Config.ResourceGroup;
        private readonly string accountName = Config.AccountName;

        /// <summary>
        /// Constructor to define how to startup and initialize Media Services
        /// </summary>
        /// <param name="azureMediaServicesClient"></param>
        public GoussanMediaService(AzureMediaServicesClient azureMediaServicesClient)
        {
            _azMediaServices = azureMediaServicesClient;
        }

        /// <summary>
        /// Function to create a new Video asset to be scheduled to be streamed by Azure Media Service
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <param name="videos"></param>
        /// <returns></returns>
        public async Task<UploadVideo> CreateAsset(IFormFile fileToUpload, UploadVideo videos)
        {
            try
            {
                // Create Asset Container in Azure Blob Storage for file storage
                Asset asset = await InternalCreate(videos.Id);
                // Get the Shared Access Signature for the Asset Container matching the Video ID
                // This SAS has a limited availability to make sure the file retains integrity and can't be modified once it has been uploaded
                // Without having access to the blob storage
                // Expiry Time on SAS set to: 5 Min UTC from time of creation
                AssetContainerSas response = await ListContainers(asset.Name);
                // Fetch the first available SAS Uri available
                Uri sasUri = new(response.AssetContainerSasUrls.First());
                // Upload the file to Azure Blob Storage with the SAS URI from Azure Media Services
                _ = await UploadFile(fileToUpload, asset.Name, sasUri);
                Asset outputAsset = await CreateOutputAsset(asset.Name);
                _ = await SubmitJobAsync(asset.Name, outputAsset.Name);
                // Create a Locator ID, Streaming Locator is fancy for URL to manifest file for HLS or DASH streaming manifest
                // ref: https://stackoverflow.com/questions/63674468/what-does-a-streaming-locator-symbolize-in-azure-media-services
                StreamingLocator locator = await CreateStreamingLocatorAsync(outputAsset.Name);
                videos.Locator = locator.Name;
                videos.OutputAsset = outputAsset.Name;
                // Return the populated video object ready to be pushed to Cosmos DB for storage
                return videos;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        /// <summary>
        /// Create the Asset for the file and return a SAS Uri to the caller for them to upload the file.
        /// </summary>
        /// <param name="videos"></param>
        /// <returns>SAS URI for uploading of File</returns>
        public async Task<UploadVideo> CreateAsset(UploadVideo videos)
        {
            try
            {
                // Create Asset Container in Azure Blob Storage for file storage
                Asset asset = await InternalCreate(videos.Id);
                videos.Assetid = asset.AssetId.ToString();
                // Get the Shared Access Signature for the Asset Container matching the Video ID
                // This SAS has a limited availability to make sure the file retains integrity and can't be modified once it has been uploaded
                // Without having access to the blob storage
                // Expiry Time on SAS set to: 5 Min UTC from time of creation
                AssetContainerSas response = await ListContainers(asset.Name);
                // Fetch the first available SAS Uri available
                Uri sasUri = new(response.AssetContainerSasUrls.FirstOrDefault()!);
                videos.Sas = sasUri.AbsoluteUri;

                Asset outputAsset = await CreateOutputAsset(asset.Name);
                videos.OutputAsset = outputAsset.Name;

                // Create a Locator ID, Streaming Locator is fancy for URL to manifest file for HLS or DASH streaming manifest
                // ref: https://stackoverflow.com/questions/63674468/what-does-a-streaming-locator-symbolize-in-azure-media-services
                StreamingLocator locator = await CreateStreamingLocatorAsync(outputAsset.Name);
                videos.Locator = locator.Name;
                // Return the populated video object ready to be pushed to Cosmos DB for storage
                return videos;
            }
            catch (Exception)
            {
                return null!;
            }
        }



        // Upload File using IFormFile
        private static async Task<Response<BlobContentInfo>> UploadFile(IFormFile file, string fileName, Uri uri)
        {
            try
            {
                Response<BlobContentInfo> response;
                // Create the blob container client using the SAS URI
                BlobContainerClient blobContainerClient = new(uri);
                // Create the blob client
                BlobClient blob = blobContainerClient.GetBlobClient(fileName);
                // Read the filestream
                using (var fs = file.OpenReadStream())
                {
                    // Upload data to Azure Blob Storage
                    response = await blob.UploadAsync(fs);
                }
                return response;
            }
            catch (RequestFailedException)
            {
                return null!;
            }
        }



        // Operations to create the asset
        private async Task<Asset> InternalCreate(string ID)
        {
            Asset asset = await _azMediaServices.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, ID, new Asset());
            return asset;
        }

        // Operations to Get a list of Containers matching the ID passed
        private async Task<AssetContainerSas> ListContainers(string ID)
        {
            AssetContainerSas response = await _azMediaServices.Assets.ListContainerSasAsync(
                resourceGroupName, 
                accountName, 
                ID,
                permissions: AssetContainerPermission.ReadWriteDelete,
                expiryTime: DateTime.UtcNow.AddMinutes(10).ToUniversalTime());
            
            return response;
        }

        private async Task<Asset> CreateOutputAsset(string ID)
        {
            // Check if asset already exist
            Asset outputAsset = await _azMediaServices.Assets.GetAsync(resourceGroupName, accountName, ID);
            Asset asset = new();
            string outputAssetName = ID;

            if (outputAsset != null)
            {
                // Name collision! time to create a new unique name for the asset
                string unique = $"-Encoded";
                outputAssetName += unique;
            }
            // Create the Asset for the encoding Job to be written to
            Asset output = await _azMediaServices.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, outputAssetName, asset);
            return output;
        }

        // Creates a Job with information about how to Encode the Asset
        public async Task<Job> SubmitJobAsync(
            string inputAsset, 
            string outputAsset, 
            string transformName = "GoussanAdaptiveStreamingPreset", 
            string jobName = "GoussanEncoding")
        {
            jobName += "-" + inputAsset;

            // Create the Input object
            JobInputAsset jobInput = new(inputAsset);
            // Create the Output object
            JobOutput[] jobOutputs =
            {
                new JobOutputAsset(outputAsset)
            };

            // Check if job already exists
            var getjob = await _azMediaServices.Jobs.GetAsync(resourceGroupName, accountName, transformName, jobName);

            if (getjob == null)
            {
                // if job doesn't exist, then we create a new job
                Job job = await _azMediaServices.Jobs.CreateAsync(resourceGroupName, accountName, transformName, jobName, new Job
                {
                    Input = jobInput,
                    Outputs = jobOutputs
                });
                // Return the job
                return job;
            }
            // Return the job
            return getjob;
        }

        // Recipe or Encoding of the content in Media Services
        public async Task<Transform> GetOrCreateTransformAsync(string transformName = "GoussanAdaptiveStreamingPreset")
        {
            // Does a Transform already exist with the desired name?
            Transform transform = await _azMediaServices.Transforms.GetAsync(resourceGroupName, accountName, transformName);

            if (transform == null)
            {
                // Specify what I need it to produce as an output
                TransformOutput[] output = new TransformOutput[]
                {
                    new TransformOutput
                    {
                        // Set the preset
                        Preset = new BuiltInStandardEncoderPreset()
                        {
                            PresetName = EncoderNamedPreset.AdaptiveStreaming
                        }
                    }
                };

                // Create the Transform with the output defined above
                transform = await _azMediaServices.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName, output);
            }
            return transform;
        }


        // Need to create a Streaming Locator for the specified asset to be available for playback for clients.
        public async Task<StreamingLocator> CreateStreamingLocatorAsync(string assetName, string locatorName = null)
        {
            if (string.IsNullOrEmpty(locatorName))
            {
                var uid = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                locatorName = uid;
            }
            StreamingLocator locator = await _azMediaServices.StreamingLocators.CreateAsync(resourceGroupName, accountName, locatorName, new StreamingLocator
            {
                AssetName = assetName,
                StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly
            });
            return locator;
        }

        // Builds the streaming URLs available, Returns null if no streaming paths available
        public async Task<IList<string>> GetStreamingURL(string locatorName)
        {
            try
            {
                IList<string> streamingUrls = new List<string>();
                ListPathsResponse paths = await _azMediaServices.StreamingLocators.ListPathsAsync(resourceGroupName, accountName, locatorName);

                foreach (StreamingPath path in paths.StreamingPaths)
                {
                    UriBuilder uriBuilder = new()
                    {
                        Scheme = "https",
                        Host = Config.StreamingEndpoint,
                        Path = path.Paths[0]
                    };
                    streamingUrls.Add(uriBuilder.ToString());
                }

                return streamingUrls;


            }
            catch (Exception)
            {
                return null!;
            }
        }


        // Ensure that streaming endpoint is online and running
        public async Task<StreamingEndpoint> EnsureStreamingEndpoint(string Endpoint = "default")
        {
            StreamingEndpoint streamingEndpoint = await _azMediaServices.StreamingEndpoints.GetAsync(resourceGroupName, accountName, Endpoint);

            if (streamingEndpoint != null)
            {
                if (streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running)
                {
                    await _azMediaServices.StreamingEndpoints.StartAsync(resourceGroupName, accountName, Endpoint);
                }
            }
            return streamingEndpoint;
        }
    }
}