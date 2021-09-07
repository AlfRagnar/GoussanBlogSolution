using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.MediaModels;
using Microsoft.Azure.Cosmos;

namespace GoussanBlogData.Services;
public interface ICosmosDbService
{
    // USER API
    Task<ItemResponse<Models.DatabaseModels.User>> AddUser(Models.DatabaseModels.User user);
    Task<Models.DatabaseModels.User> GetUserAsync(string id);
    IQueryable<dynamic> GetUsersAsync();
    Task<string> ConfirmUser(string token);

    Task<IEnumerable<Models.DatabaseModels.User>> GetUserByName(string Username);
    Task<IEnumerable<Models.DatabaseModels.User>> GetUserByMail(string Email);
    Task<IEnumerable<Models.DatabaseModels.User>> CheckUser(string Username, string Email);

    // VIDEO API
    Task AddVideo(UploadVideo video);
    Task DeleteVideoAsync(string id);
    Task<IEnumerable<UploadVideo>> GetMultipleVideosAsync(string queryString);
    Task<UploadVideo> GetVideoAsync(string id);
    Task UpdateVideoAsync(string id, VideoUpdateModel video);
    Task UpdateVideoAsync(string id, UploadVideo video);
    Task<IEnumerable<UploadVideo>> GetVideoList();
    // IMAGE API

    /// <summary>
    /// Adds a Image Object to COSMOS DB and returns a response to creation request
    /// </summary>
    /// <param name="image"></param>
    /// <returns>Response to Creation Request</returns>
    Task<ItemResponse<Image>> AddImage(Image image);
    Task<List<Image>> GetImageList();
    Task UpdateImageAsync(string id, Image image);

    // CHAT API
    
    /// <summary>
    /// Tries to get chat history from Database
    /// </summary>
    /// <returns></returns>
    Task<List<LoggedMessage>> GetChatHistory();
    /// <summary>
    /// Adds a message to the Database for storage
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task AddMessage(LoggedMessage message);
    


    // BLOG API
}
