
using GoussanBlogData.Models;
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

    public async Task<IEnumerable<Models.User>> GetUsersAsync(string queryString)
    {
        try
        {
            FeedIterator<Models.User> query = MediaContainer.GetItemQueryIterator<Models.User>(new QueryDefinition(queryString));
            List<Models.User> results = new();
            while (query.HasMoreResults)
            {
                FeedResponse<Models.User> response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
    public async Task<ItemResponse<Models.User>> AddUser(Models.User user)
    {
        var res = await UserContainer.CreateItemAsync(user, new PartitionKey(user.Id));
        return res;
    }
    public async Task<Models.User> GetUserAsync(string id)
    {
        try
        {
            var response = await MediaContainer.ReadItemAsync<Models.User>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }
    public async Task<IEnumerable<Models.User>> GetUserByName(string Username)
    {
        try
        {
            List<Models.User> result = new();
            using (FeedIterator<Models.User> setIterator = UserContainer.GetItemLinqQueryable<Models.User>().Where(x => x.UserName == Username).ToFeedIterator())
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
            return null!;
        }
    }
    //public async Task<IEnumerable<Models.User>> GetUserByName(string Username)
    //{
    //    try
    //    {
    //        FeedIterator<Models.User> query = MediaContainer.GetItemQueryIterator<Models.User>(new QueryDefinition($"SELECT * FROM c WHERE c.username = {Username}"));
    //        List<Models.User> results = new();
    //        while (query.HasMoreResults)
    //        {
    //            FeedResponse<Models.User> response = await query.ReadNextAsync();
    //            results.AddRange(response.ToList());
    //        }
    //        return results;
    //    }
    //    catch (CosmosException ex)
    //    {
    //        Console.WriteLine(ex);
    //        return null!;
    //    }
    //}

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
