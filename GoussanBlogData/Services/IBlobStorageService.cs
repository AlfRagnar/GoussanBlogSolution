using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace GoussanBlogData.Services
{
    public interface IBlobStorageService
    {
        Task<Uri> UploadFileToStorage(Stream stream, string container, string fileName);

        Task<BlobProperties> GetBlobPropertiesAsync(BlobClient blob);

        IAsyncEnumerable<Page<BlobHierarchyItem>> ListBlobsPublic(BlobContainerClient blobContainerClient, int? segmentSize);

        Task<BlobContainerClient> GetContainer(string name);

        BlobClient RetrieveBlobAsync(string id);
        Response<bool> DeleteVideo(string videoName);
        // BLOB STORAGE IMAGE API
        Task<string> UploadImage(IFormFile file, string imageName);
    }
}