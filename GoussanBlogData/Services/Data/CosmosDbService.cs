using GoussanBlogData.Models.MediaModels;
using Goussanjarga.Models;
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

    public async Task<IEnumerable<Models.UserModels.User>> GetUsersAsync(string queryString)
    {
        try
        {
            FeedIterator<Models.UserModels.User> query = MediaContainer.GetItemQueryIterator<Models.UserModels.User>(new QueryDefinition(queryString));
            List<Models.UserModels.User> results = new();
            while (query.HasMoreResults)
            {
                FeedResponse<Models.UserModels.User> response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
    public async Task<ItemResponse<Models.UserModels.User>> AddUser(Models.UserModels.User user)
    {
        var res = await UserContainer.CreateItemAsync(user, new PartitionKey(user.Id));
        return res;
    }
    public async Task<Models.UserModels.User> GetUserAsync(string id)
    {
        try
        {
            var response = await MediaContainer.ReadItemAsync<Models.UserModels.User>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
    public async Task<IEnumerable<Models.UserModels.User>> GetUserByName(string Username)
    {
        try
        {
            List<Models.UserModels.User> result = new();
            using (FeedIterator<Models.UserModels.User> setIterator = UserContainer.GetItemLinqQueryable<Models.UserModels.User>().Where(x => x.UserName == Username).ToFeedIterator())
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
            return null!;
        }
    }
    public async Task<IEnumerable<Models.UserModels.User>> GetUserByMail(string Email)
    {
        try
        {
            List<Models.UserModels.User> result = new();
            using (FeedIterator<Models.UserModels.User> setIterator = UserContainer.GetItemLinqQueryable<Models.UserModels.User>().Where(x => x.Email == Email).ToFeedIterator())
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
            return null!;
        }
    }
    public async Task<IEnumerable<Models.UserModels.User>> CheckUser(string Username, string Email)
    {
        try
        {
            List<Models.UserModels.User> result = new();
            using (FeedIterator<Models.UserModels.User> setIterator = UserContainer.GetItemLinqQueryable<Models.UserModels.User>().Where(x => x.Email == Email || x.UserName == Username).ToFeedIterator())
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
            return null!;
        }
    }


    public async Task AddVideo(Video video)
    {
        await MediaContainer.CreateItemAsync(video, new PartitionKey(video.Id));
    }

    public async Task DeleteVideoAsync(string id)
    {
        await MediaContainer.DeleteItemAsync<Video>(id, new PartitionKey(id));
    }

    public async Task<Video> GetVideoAsync(string id)
    {
        try
        {
            var response = await MediaContainer.ReadItemAsync<Video>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<Video>> GetMultipleVideosAsync(string queryString)
    {
        try
        {
            FeedIterator<Video> query = MediaContainer.GetItemQueryIterator<Video>(new QueryDefinition(queryString));
            List<Video> results = new();
            while (query.HasMoreResults)
            {
                FeedResponse<Video> response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    public async Task UpdateVideoAsync(string id, Video video)
    {
        await MediaContainer.UpsertItemAsync(video, new PartitionKey(id));
    }
}
