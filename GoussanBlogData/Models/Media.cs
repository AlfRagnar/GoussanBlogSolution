using System.Text.Json.Serialization;

namespace GoussanBlogData.Models;

public class Media
{
    [JsonPropertyName("userid")]
    public string UserId { get; set;  }
    [JsonPropertyName("images")]
    public List<Image> Images { get; set; }
    [JsonPropertyName("videos")]
    public List<Video> Video { get; set; }
    [JsonPropertyName("blogposts")]
    public List<BlogPost> BlogPosts { get; set; }
}
