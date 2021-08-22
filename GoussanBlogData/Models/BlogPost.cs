using System.Text.Json.Serialization;

namespace GoussanBlogData.Models;
public class BlogPost
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
    [JsonPropertyName("videos")]
    public List<Video> Videos { get; set; }
    [JsonPropertyName("images")]
    public List<Video> Images { get; set; }
}
