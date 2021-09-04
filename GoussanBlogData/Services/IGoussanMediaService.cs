using GoussanBlogData.Models.DatabaseModels;
using Microsoft.Azure.Management.Media.Models;

namespace GoussanBlogData.Services
{
    public interface IGoussanMediaService
    {
        Task<UploadVideo> CreateAsset(IFormFile fileToUpload, UploadVideo video);

        Task<StreamingLocator> CreateStreamingLocatorAsync(string assetName, string locatorName = null);

        Task<StreamingEndpoint> EnsureStreamingEndpoint(string Endpoint = "default");

        Task<Transform> GetOrCreateTransformAsync(string transformName = "GoussanAdaptiveStreamingPreset");

        Task<IList<string>> GetStreamingURL(string locatorName);
    }
}