using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

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
                BlobContainerClient container = await _blobServiceClient.CreateBlobContainerAsync(ID);
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



        public async Task<Uri> UploadFileToStorage(Stream stream, string container, string fileName)
        {
            string newFileName = Guid.Parse(fileName).ToString();
            Uri blobUri = new(_blobServiceClient.Uri + container + newFileName);
            BlobClient blobClient = new(blobUri);

            await blobClient.UploadAsync(stream);
            return blobUri;
        }


        public Task Save(Stream fileStream, string name)
        {
            throw new NotImplementedException();
        }
    }
}