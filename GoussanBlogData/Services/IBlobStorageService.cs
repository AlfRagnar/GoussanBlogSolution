using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace GoussanBlogData.Services
{
    /// <summary>
    /// Interface handling all the interactions with Blob Storage
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Define required properties before a file is allowed to be uploaded to Storage
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="container"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<Uri> UploadFileToStorage(Stream stream, string container, string fileName);
        /// <summary>
        /// Tries to get the properties from a blob client
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        Task<BlobProperties> GetBlobPropertiesAsync(BlobClient blob);
        /// <summary>
        /// List all public blobs within a blob container
        /// </summary>
        /// <param name="blobContainerClient"></param>
        /// <param name="segmentSize"></param>
        /// <returns></returns>
        IAsyncEnumerable<Page<BlobHierarchyItem>> ListBlobsPublic(BlobContainerClient blobContainerClient, int? segmentSize);
        /// <summary>
        /// Tries to create or get a blob container client 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Blob Container Client</returns>
        Task<BlobContainerClient> GetContainer(string name);
        /// <summary>
        /// Tries to retrieve a blob with the ID provided
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BlobClient RetrieveBlobAsync(string id);
        /// <summary>
        /// Tries to delete a video
        /// </summary>
        /// <param name="videoName"></param>
        /// <returns></returns>
        Response<bool> DeleteVideo(string videoName);
        // BLOB STORAGE IMAGE API
        /// <summary>
        /// Uploads a Image to Blob Storage
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        Task<string> UploadImage(IFormFile file, string imageName);

        /// <summary>
        /// Generate a SAS URI for supplied container
        /// </summary>
        /// <param name="containerClient"></param>
        /// <param name="storedPolicyName"></param>
        /// <returns></returns>
        Uri GetServiceSasUriForContainer(BlobContainerClient containerClient, string storedPolicyName = null);
        /// <summary>
        /// Generates a SAS URI for the supplied Blob
        /// </summary>
        /// <param name="blobClient"></param>
        /// <param name="storedPolicyName"></param>
        /// <returns></returns>
        Uri GetServiceSasUriForBlob(BlobClient blobClient, string storedPolicyName = null);
    }
}