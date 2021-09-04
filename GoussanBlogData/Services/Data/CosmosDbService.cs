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
    private readonly CosmosClient cosmosClient;
    private readonly Database dbService;
    public CosmosDbService(CosmosClient cosmosClient, string databaseName)
    {
        this.cosmosClient = cosmosClient;
        dbService = cosmosClient.GetDatabase(databaseName);
        MediaContainer = dbService.GetContainer(Config.CosmosMedia);
        UserContainer = dbService.GetContainer(Config.CosmosUser);
    }


    // USER API
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
    public async Task<ItemResponse<Models.DatabaseModels.User>> AddUser(Models.DatabaseModels.User user)
    {
        var res = await UserContainer.CreateItemAsync(user, new PartitionKey(user.Id));
        return res;
    }
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

    // Try to Activate User
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

    public async Task AddVideo(UploadVideo video)
    {
        await MediaContainer.CreateItemAsync(video, new PartitionKey(video.Id));
    }

    public async Task DeleteVideoAsync(string id)
    {
        await MediaContainer.DeleteItemAsync<UploadVideo>(id, new PartitionKey(id));
    }

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
    public async Task UpdateVideoAsync(string id, UploadVideo video)
    {
        await MediaContainer.UpsertItemAsync(video, new PartitionKey(id));
    }

    // IMAGE API
    public async Task AddImage(Image image)
    {
        await MediaContainer.CreateItemAsync(image, new PartitionKey(image.Id));
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
            return null!;
        }
    }

    public async Task UpdateImageAsync(string id, Image image)
    {
        await MediaContainer.UpsertItemAsync(image, new PartitionKey(id));
    }

}
