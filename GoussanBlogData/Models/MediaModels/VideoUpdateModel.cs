using Newtonsoft.Json;

namespace GoussanBlogData.Models.MediaModels;
public class VideoUpdateModel
{
    // REQUIRED
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("userid")]
    public string UserId { get; set; }
    

    // OPTIONAL
    [JsonProperty("blogid")]
    public string? BlogId { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }

}

