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
    /// <summary>
    /// Request to delete a Video Object stored in Cosmos DB
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Response to Request</returns>
    Task<ItemResponse<UploadVideo>> DeleteVideoAsync(string id);
    Task<IEnumerable<UploadVideo>> GetMultipleVideosAsync(string queryString);
    Task<UploadVideo> GetVideoAsync(string id);
    Task UpdateVideoAsync(string id, VideoUpdateModel video);
    Task UpdateVideoAsync(string id, UploadVideo video);
    Task<IEnumerable<UploadVideo>> GetVideoList();
    /// <summary>
    /// Request to retrieve videos that have not finished encoding
    /// </summary>
    /// <returns>List of Videos that have not finished encoding</returns>
    Task<IEnumerable<UploadVideo>> GetNotFinishedVideosAsync();
    /// <summary>
    /// Request to retrieve a list of all videos stored in database
    /// </summary>
    /// <returns>List of all videos stored in database</returns>
    Task<IEnumerable<UploadVideo>> GetAllVideosAsync();
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
    /// <summary>
    /// Call to Database to get a list of blogs available
    /// </summary>
    /// <returns>List of Blog Posts</returns>
    Task<List<BlogPost>> GetBlogs();
    /// <summary>
    /// Call to Database to retrieve blogs with matching ID, can be more than one
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<BlogPost>> GetBlogs(int id);
    /// <summary>
    /// Request to create a new blog post object in the database
    /// </summary>
    /// <param name="post"></param>
    /// <returns>Response from Cosmos</returns>
    Task<ItemResponse<BlogPost>> CreateBlogPost(BlogPost post);
}
