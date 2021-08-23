
using GoussanBlogData.Models;
using Microsoft.Azure.Cosmos;

namespace GoussanBlogData.Services;
public interface ICosmosDbService
{
    // USER API
    Task<ItemResponse<Models.User>> AddUser(Models.User user);
    Task<Models.User> GetUserAsync(string id);
    Task<IEnumerable<Models.User>> GetUsersAsync(string queryString);

    // VIDEO API
    Task AddVideo(Video video);
    Task DeleteVideoAsync(string id);
    Task<IEnumerable<Video>> GetMultipleVideosAsync(string queryString);
    Task<Video> GetVideoAsync(string id);
    Task UpdateVideoAsync(string id, Video video);
    Task<IEnumerable<Models.User>> GetUserByName(string Username);
}
