using GoussanBlogData.Models;
using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.MediaModels;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace GoussanBlogData.Services;
public class CosmosDbService : ICosmosDbService
{
    private readonly Container MediaContainer;
    private readonly Container UserContainer;
    private readonly Container ChatContainer;
    private readonly CosmosClient cosmosClient;
    private readonly Database dbService;
    public CosmosDbService(CosmosClient cosmosClient, string databaseName)
    {
        this.cosmosClient = cosmosClient;
        dbService = cosmosClient.GetDatabase(databaseName);
        MediaContainer = dbService.GetContainer(Config.CosmosMedia);
        UserContainer = dbService.GetContainer(Config.CosmosUser);
        ChatContainer = dbService.GetContainer(Config.CosmosChat);
    }


    // USER API

    /// <summary>
    /// Request to get a list of users stored in database
    /// Returns only the Username and the date when they were created
    /// </summary>
    /// <returns></returns>
    public IQueryable<dynamic> GetUsersAsync()
    {
        try
        {
            dynamic Users = UserContainer
                .GetItemLinqQueryable<Models.DatabaseModels.User>(allowSynchronousQueryExecution: true)
                .Select(x => new
                {
                    username = x.Username,
                    created = x.Created
                });
            return Users;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
    /// <summary>
    /// Adds a user to the Database
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<ItemResponse<Models.DatabaseModels.User>> AddUser(Models.DatabaseModels.User user)
    {
        var res = await UserContainer.CreateItemAsync(user, new PartitionKey(user.Id));
        return res;
    }
    /// <summary>
    /// Tries to get a user by the ID submitted
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Models.DatabaseModels.User> GetUserAsync(string id)
    {
        try
        {
            var response = await UserContainer.ReadItemAsync<Models.DatabaseModels.User>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
    /// <summary>
    /// Tries to get the user by their Username in the database
    /// </summary>
    /// <param name="Username"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Models.DatabaseModels.User>> GetUserByName(string Username)
    {
        try
        {
            List<Models.DatabaseModels.User> result = new();
            using (FeedIterator<Models.DatabaseModels.User> setIterator = UserContainer
                .GetItemLinqQueryable<Models.DatabaseModels.User>()
                .Where(x => x.Username == Username)
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    /// <summary>
    /// Tries to get a user by Email submitted
    /// </summary>
    /// <param name="Email"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Models.DatabaseModels.User>> GetUserByMail(string Email)
    {
        try
        {
            List<Models.DatabaseModels.User> result = new();
            using (FeedIterator<Models.DatabaseModels.User> setIterator = UserContainer
                .GetItemLinqQueryable<Models.DatabaseModels.User>()
                .Where(x => x.Email == Email)
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    /// <summary>
    /// Checks if there is any user currently in database with matching username or email
    /// </summary>
    /// <param name="Username"></param>
    /// <param name="Email"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Models.DatabaseModels.User>> CheckUser(string Username, string Email)
    {
        try
        {
            List<Models.DatabaseModels.User> result = new();
            using (FeedIterator<Models.DatabaseModels.User> setIterator = UserContainer
                .GetItemLinqQueryable<Models.DatabaseModels.User>()
                .Where(x => x.Email == Email || x.Username == Username)
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }


    /// <summary>
    /// Tries to activate the user
    /// </summary>
    /// <param name="Token"></param>
    /// <returns></returns>
    public async Task<string> ConfirmUser(string Token)
    {
        try
        {
            if (string.IsNullOrEmpty(Token))
            {
                return null!;
            }
            string response = null!;
            List<Models.DatabaseModels.User> result = new();
            using (FeedIterator<Models.DatabaseModels.User> setIterator = UserContainer
                .GetItemLinqQueryable<Models.DatabaseModels.User>()
                .Where(x => x.Confirmationcode == Token)
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            if (result.Any())
            {
                var user = result.First();
                if (user.Status != "Activated")
                {
                    user.Status = "Activated";
                    user.Confirmationcode = null!;
                    await UserContainer.UpsertItemAsync(user, new PartitionKey(user.Id));
                    response = "User Activated";
                }
            }
            return response!;
        }
        catch (Exception)
        {
            return null!;
        }

    }

    // VIDEO API

    /// <summary>
    /// Adds a new video object to the database, does not contain any raw video data
    /// </summary>
    /// <param name="video"></param>
    /// <returns></returns>
    public async Task AddVideo(UploadVideo video)
    {
        await MediaContainer.CreateItemAsync(video, new PartitionKey(video.Id));
    }

    /// <summary>
    /// Removes a video object from the database, does not remove any raw video data
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    public async Task<ItemResponse<UploadVideo>> DeleteVideoAsync(string id)
    {
        try
        {
            var res = await MediaContainer.DeleteItemAsync<UploadVideo>(id, new PartitionKey(id));
            return res;

        }
        catch (CosmosException ex)
        {
            return null!;
        }
    }

    /// <summary>
    /// Tries to find a video object by the ID provided
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UploadVideo> GetVideoAsync(string id)
    {
        try
        {
            var response = await MediaContainer.ReadItemAsync<UploadVideo>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    /// <summary>
    /// Returns a list of videos in the database with the State set to "Finished"
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<UploadVideo>> GetVideoList()
    {
        try
        {
            List<UploadVideo> result = new();
            using (FeedIterator<UploadVideo> setIterator = MediaContainer
                .GetItemLinqQueryable<UploadVideo>()
                .Where(x => x.Type == "Video" && x.State == "Finished")
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
    /// <summary>
    /// Retrieves Videos from Database that have not finished encoding
    /// </summary>
    /// <returns>List of Videos that have not finished Encoding</returns>
    public async Task<IEnumerable<UploadVideo>> GetNotFinishedVideosAsync()
    {
        try
        {
            List<UploadVideo> result = new();
            using (FeedIterator<UploadVideo> setIterator = MediaContainer
                .GetItemLinqQueryable<UploadVideo>()
                .Where(x => x.Type == "Video" && x.State != "Finished")
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
    /// <summary>
    /// Retrieves List of Videos stored in Database
    /// </summary>
    /// <returns>List of Videos</returns>
    public async Task<IEnumerable<UploadVideo>> GetAllVideosAsync()
    {
        try
        {
            List<UploadVideo> result = new();
            using (FeedIterator<UploadVideo> setIterator = MediaContainer
                .GetItemLinqQueryable<UploadVideo>()
                .Where(x => x.Type == "Video")
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    /// <summary>
    /// Tries to get multiple videos from the database with the supplied query
    /// </summary>
    /// <param name="queryString"></param>
    /// <returns></returns>
    public async Task<IEnumerable<UploadVideo>> GetMultipleVideosAsync(string queryString)
    {
        try
        {
            FeedIterator<UploadVideo> query = MediaContainer
                .GetItemQueryIterator<UploadVideo>(new QueryDefinition(queryString));
            List<UploadVideo> results = new();
            while (query.HasMoreResults)
            {
                FeedResponse<UploadVideo> response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }


    /// <summary>
    /// Updates a video object in the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="video"></param>
    /// <returns></returns>
    public async Task UpdateVideoAsync(string id, VideoUpdateModel video)
    {
        var documentResource = await MediaContainer.ReadItemAsync<UploadVideo>(id, new PartitionKey(id));
        var document = documentResource.Resource;
        if (video.Description != null)
        {
            document.Description = video.Description;
        }
        if (video.Title != null)
        {
            document.Title = video.Title;
        }
        document.Updated = DateTime.UtcNow.ToString();
        await MediaContainer.UpsertItemAsync(document, new PartitionKey(id));
    }
    /// <summary>
    /// Updates or Adds a video to the database 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="video"></param>
    /// <returns></returns>
    public async Task UpdateVideoAsync(string id, UploadVideo video)
    {
        await MediaContainer.UpsertItemAsync(video, new PartitionKey(id));
    }

    // IMAGE API
    public async Task<ItemResponse<Image>> AddImage(Image image)
    {
        var res = await MediaContainer.CreateItemAsync(image, new PartitionKey(image.Id));
        return res;
    }

    public async Task<List<Image>> GetImageList()
    {
        try
        {
            List<Image> result = new();
            using (FeedIterator<Image> setIterator = MediaContainer
                .GetItemLinqQueryable<Image>()
                .Where(x => x.Type == "Image")
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        catch (CosmosException)
        {
            return null;
        }
    }

    public async Task UpdateImageAsync(string id, Image image)
    {
        await MediaContainer.UpsertItemAsync(image, new PartitionKey(id));
    }


    // Chat API

    /// <summary>
    /// Tries to get the chat history from the database
    /// </summary>
    /// <returns></returns>
    public async Task<List<LoggedMessage>> GetChatHistory()
    {
        try
        {
            FeedIterator<LoggedMessage> query = ChatContainer
                .GetItemQueryIterator<LoggedMessage>(new QueryDefinition("SELECT TOP 20 * FROM c ORDER BY c._ts DESC"));
            List<LoggedMessage> results = new();
            while (query.HasMoreResults)
            {
                FeedResponse<LoggedMessage> response = await query.ReadNextAsync();
                results.AddRange(response.Resource);
            }
            return results;
        }
        catch (CosmosException ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    /// <summary>
    /// Tries to add a message to the Chat container in the database
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>

    public async Task AddMessage(LoggedMessage message)
    {
        try
        {
            await ChatContainer.CreateItemAsync<LoggedMessage>(message, new PartitionKey(message.Id));
        }
        catch (CosmosException)
        {
            throw;
        }
    }


    // BLOG API

    /// <summary>
    /// Call to Retrieve a list of blogs from the database
    /// </summary>
    /// <returns>List of Blogs</returns>
    public async Task<List<BlogPost>> GetBlogs()
    {
        try
        {
            List<BlogPost> result = new();
            using (FeedIterator<BlogPost> setIterator = MediaContainer
                .GetItemLinqQueryable<BlogPost>()
                .Where(x => x.Type == "Blog")
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        catch (CosmosException ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    /// <summary>
    /// Tries to get a Specific blog post
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Returns specified blog post</returns>
    public async Task<List<BlogPost>> GetBlogs(int id)
    {
        try
        {
            List<BlogPost> result = new();
            using (FeedIterator<BlogPost> setIterator = MediaContainer
                .GetItemLinqQueryable<BlogPost>()
                .Where(x => x.Id == id.ToString())
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        catch (CosmosException ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<ItemResponse<BlogPost>> CreateBlogPost(BlogPost post)
    {
        try
        {
            var response = await MediaContainer.CreateItemAsync<BlogPost>(post, new PartitionKey(post.Id));
            return response;
        }
        catch (CosmosException ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}
