using Newtonsoft.Json;

namespace GoussanBlogData.Models.DatabaseModels;
public class BlogPost
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("userid")]
    public string UserId { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("content")]
    public string Content { get; set; }
    [JsonProperty("videos")]
    public List<UploadVideo> Videos { get; set; }
    [JsonProperty("images")]
    public List<UploadVideo> Images { get; set; }
}
