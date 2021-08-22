
using GoussanBlogData.Models;
using Microsoft.Azure.Cosmos;

namespace GoussanBlogData.Services;
public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;
    public CosmosDbService(CosmosClient cosmosDbClient, string databaseName, string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public async Task AddAsync(Video video)
    {
        await _container.CreateItemAsync(video, new PartitionKey(video.id));
    }

    public async Task DeleteAsync(string id)
    {
        await _container.DeleteItemAsync<Video>(id, new PartitionKey(id));
    }

    public async Task<Video> GetAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<Video>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<Video>> GetMultipleAsync(string queryString)
    {
        try
        {
            FeedIterator<Video> query = _container.GetItemQueryIterator<Video>(new QueryDefinition(queryString));
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

    public async Task UpdateAsync(string id, Video video)
    {
        await _container.UpsertItemAsync(video, new PartitionKey(id));
    }
}
