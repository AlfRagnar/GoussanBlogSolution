using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.MediaModels;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoussanBlogData.Services;
public interface ICosmosDbService
{
    // USER API
    Task<ItemResponse<Models.DatabaseModels.User>> AddUser(Models.DatabaseModels.User user);
    Task<Models.DatabaseModels.User> GetUserAsync(string id);
    Task<IEnumerable<Models.DatabaseModels.User>> GetUsersAsync(string queryString);
    Task<IEnumerable<Models.DatabaseModels.User>> GetUserByName(string Username);
    Task<IEnumerable<Models.DatabaseModels.User>> GetUserByMail(string Email);
    Task<IEnumerable<Models.DatabaseModels.User>> CheckUser(string Username, string Email);

    // VIDEO API
    Task AddVideo(UploadVideo video);
    Task DeleteVideoAsync(string id);
    Task<IEnumerable<UploadVideo>> GetMultipleVideosAsync(string queryString);
    Task<UploadVideo> GetVideoAsync(string id);
    Task UpdateVideoAsync(string id, VideoCreateModel video);
    Task<IEnumerable<UploadVideo>> GetVideoList();
    // IMAGE API
    Task AddImage(Image image);
    Task<List<Image>> GetImageList();
    Task UpdateImageAsync(string id, Image image);


    // BLOG API
}
