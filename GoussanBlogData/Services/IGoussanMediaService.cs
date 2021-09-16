using GoussanBlogData.Models.DatabaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Management.Media.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoussanBlogData.Services
{
    /// <summary>
    /// Interface definition on functions available to be called to interact with Azure Media Services
    /// </summary>
    public interface IGoussanMediaService
    {
        /// <summary>
        /// Creates and Uploads a file to Azure Storage
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <param name="video"></param>
        /// <returns>Video Object updated with fields filled from the Task</returns>
        Task<UploadVideo> CreateAsset(IFormFile fileToUpload, UploadVideo video);
        /// <summary>
        /// Creates a new Asset that Media Services can work with
        /// </summary>
        /// <param name="video"></param>
        /// <returns>Updated Video Object with fields filled out properties updated</returns>
        Task<UploadVideo> CreateAsset(UploadVideo video);
        /// <summary>
        /// Creates a new Streaming Locator
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="locatorName"></param>
        /// <returns>Streaming Locator</returns>
        Task<StreamingLocator> CreateStreamingLocatorAsync(string assetName, string locatorName = null);
        /// <summary>
        /// Function to ensure that the Streaming Endpoint is online
        /// </summary>
        /// <param name="Endpoint"></param>
        /// <returns>Streaming Endpoint</returns>

        Task<StreamingEndpoint> EnsureStreamingEndpoint(string Endpoint = "default");
        /// <summary>
        /// Request to get or create a new Transform
        /// </summary>
        /// <param name="transformName"></param>
        /// <returns></returns>

        Task<Transform> GetOrCreateTransformAsync(string transformName = "GoussanAdaptiveStreamingPreset");
        /// <summary>
        /// Request to retrieve a URL to get a video Stream
        /// </summary>
        /// <param name="locatorName"></param>
        /// <returns></returns>
        Task<IList<string>> GetStreamingURL(string locatorName);
        /// <summary>
        /// Request to start an Encoding Task
        /// </summary>
        /// <param name="inputAsset"></param>
        /// <param name="outputAsset"></param>
        /// <param name="transformName"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        Task<Job> SubmitJobAsync(
            string inputAsset,
            string outputAsset,
            string transformName = "GoussanAdaptiveStreamingPreset",
            string jobName = "GoussanEncoding");
    }
}