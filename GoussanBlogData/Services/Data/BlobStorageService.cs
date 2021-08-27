using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoussanBlogData.Services.Data
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService(BlobServiceClient blobServiceClient, string Container)
        {
            _blobServiceClient = blobServiceClient;
            _blobContainerClient = GetContainer(Container).GetAwaiter().GetResult();
        }

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
            }
            return null;
        }

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

        public BlobClient RetrieveBlobAsync(string id)
        {
            try
            {
                BlobClient blobClient = _blobContainerClient.GetBlobClient(id);
                return blobClient;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> UploadImage(IFormFile file, string imageName)
        {
            var blob = _blobContainerClient.GetBlobClient(imageName);

            using (var fs = file.OpenReadStream())
            {
                await blob.UploadAsync(fs, new BlobHttpHeaders { ContentType = file.ContentType });
            }
            string blobUri = blob.Uri.AbsoluteUri;
            return blobUri;
        }
        public Response<bool> DeleteVideo(string videoName)
        {
            var blob = _blobContainerClient.GetBlobClient(videoName);
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