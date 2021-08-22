
using GoussanBlogData.Models;

namespace GoussanBlogData.Services;
public interface ICosmosDbService
{
    Task AddVideo(Video video);
    Task DeleteVideoAsync(string id);
    Task<IEnumerable<Video>> GetMultipleVideosAsync(string queryString);
    Task<Video> GetVideoAsync(string id);
    Task UpdateVideoAsync(string id, Video video);
}
