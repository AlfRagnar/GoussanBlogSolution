using Newtonsoft.Json;

namespace GoussanBlogData.Models;
public class BlogPost
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("userid")]
    public string UserId { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("content")]
    public string Content { get; set; }
    [JsonProperty("videos")]
    public List<Video> Videos { get; set; }
    [JsonProperty("images")]
    public List<Video> Images { get; set; }
}
