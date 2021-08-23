
using GoussanBlogData.Models;
using Goussanjarga.Models;
using Microsoft.Azure.Cosmos;

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
        MediaContainer = dbService.GetContainer(Config.CosmosUser);
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
