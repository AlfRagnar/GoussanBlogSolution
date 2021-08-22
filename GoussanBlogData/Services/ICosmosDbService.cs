
using GoussanBlogData.Models;

namespace GoussanBlogData.Services;
public interface ICosmosDbService
{
    Task AddAsync(Video video);
    Task UpdateAsync(string id, Video video);
    Task DeleteAsync(string id);
    Task<Video> GetAsync(string id);
    Task<IEnumerable<Video>> GetMultipleAsync(string queryString);
}
