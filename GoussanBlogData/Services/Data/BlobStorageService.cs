using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoussanBlogData.Services.Data
{

    /// <summary>
    /// This file contains the functions/jobs/tasks available for BlobStorageService
    /// </summary>
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _MediaBlobContainerClient;

        /// <summary>
        /// Constructor function needed to initialize the Service
        /// </summary>
        /// <param name="blobServiceClient"></param>
        /// <param name="MediaContainer"></param>
        public BlobStorageService(BlobServiceClient blobServiceClient, string MediaContainer)
        {
            _blobServiceClient = blobServiceClient;
            _MediaBlobContainerClient = GetContainer(MediaContainer).GetAwaiter().GetResult();
        }


        /// <summary>
        /// Used to try to get a blob container with a specific ID or create the container if the container doesn't exist. This is only run on startup to ensure that the containers are present and ready to receive requests.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<BlobContainerClient> GetContainer(string ID)
        {
            try
            {
                BlobContainerClient container = _blobServiceClient.CreateBlobContainer(ID);
                var createResponse = await container.CreateIfNotExistsAsync();
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                {
                    await container.SetAccessPolicyAsync(PublicAccessType.Blob);
                    return container;
                }
                if (await container.ExistsAsync())
                {
                    return container;
                }
            }
            catch (RequestFailedException ex)
            {
                if (ex.ErrorCode == BlobErrorCode.ContainerAlreadyExists)
                {
                    var container = _blobServiceClient.GetBlobContainerClient(ID);
                    return container;
                }
                Console.WriteLine(ex.Data);
            }
            return null;
        }

        /// <summary>
        /// Lists Blobs within a container that is available with public access
        /// </summary>
        /// <param name="blobContainerClient"></param>
        /// <param name="segmentSize"></param>
        /// <returns></returns>
        public IAsyncEnumerable<Page<BlobHierarchyItem>> ListBlobsPublic(BlobContainerClient blobContainerClient, int? segmentSize)
        {
            try
            {
                var resultSegment = blobContainerClient.GetBlobsByHierarchyAsync(prefix: "public").AsPages(default, segmentSize);
                return resultSegment;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the Blob properties of the blob passed in
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        public async Task<BlobProperties> GetBlobPropertiesAsync(BlobClient blob)
        {
            try
            {
                BlobProperties blobProperties = await blob.GetPropertiesAsync();
                return blobProperties;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }


        /// <summary>
        /// Tries to get a blob within the Image Container
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Blob Client</returns>
        public BlobClient RetrieveBlobAsync(string id)
        {
            try
            {
                BlobClient blobClient = _MediaBlobContainerClient.GetBlobClient(id);
                return blobClient;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Uploads image to Blob Storage
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <returns>URL with public access to file</returns>
        public async Task<string> UploadImage(IFormFile file, string imageName)
        {
            var blob = _MediaBlobContainerClient.GetBlobClient(imageName);

            using (var fs = file.OpenReadStream())
            {
                await blob.UploadAsync(fs, new BlobHttpHeaders { ContentType = file.ContentType });
            }
            string blobUri = blob.Uri.ToString();
            return blobUri;
        }

        public Response<bool> DeleteVideo(string videoName)
        {
            var blob = _MediaBlobContainerClient.GetBlobClient(videoName);
            var res = blob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
            return res;
        }


        /// <summary>
        /// Uploads File to Azure Blob Storage
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="container"></param>
        /// <param name="fileName"></param>
        /// <returns>URI to the blob uploaded</returns>
        public async Task<Uri> UploadFileToStorage(Stream stream, string container, string fileName)
        {
            string newFileName = Guid.Parse(fileName).ToString();
            Uri blobUri = new(_blobServiceClient.Uri + container + newFileName);
            BlobClient blobClient = new(blobUri);

            await blobClient.UploadAsync(stream);
            return blobUri;
        }

        /// <summary>
        /// Upserts a Blob
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task Save(Stream fileStream, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates or Retrieves a SAS Uri for the supplied COntainer
        /// </summary>
        /// <param name="containerClient"></param>
        /// <param name="storedPolicyName"></param>
        /// <returns></returns>
        public Uri GetServiceSasUriForContainer(BlobContainerClient containerClient, string storedPolicyName = null)
        {
            // Check whether this BlobContainerClient object has been authorized with Shared Key.
            if (containerClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerClient.Name,
                    Resource = "c"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30);
                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Write);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = containerClient.GenerateSasUri(sasBuilder);
                Console.WriteLine("SAS URI for blob container is: {0}", sasUri);
                Console.WriteLine();

                return sasUri;
            }
            else
            {
                Console.WriteLine(@"BlobContainerClient must be authorized with Shared Key 
                          credentials to create a service SAS.");
                return null;
            }
        }

        /// <summary>
        /// Get a Service SAS URI for a Blob
        /// </summary>
        /// <param name="blobClient"></param>
        /// <param name="storedPolicyName"></param>
        /// <returns></returns>
        public Uri GetServiceSasUriForBlob(BlobClient blobClient, string storedPolicyName = null)
        {
            // Check whether this BlobClient object has been authorized with Shared Key.
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                Console.WriteLine("SAS URI for blob is: {0}", sasUri);
                Console.WriteLine();

                return sasUri;
            }
            else
            {
                Console.WriteLine(@"BlobClient must be authorized with Shared Key 
                          credentials to create a service SAS.");
                return null;
            }
        }
    }
}