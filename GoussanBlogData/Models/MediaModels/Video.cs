using Newtonsoft.Json;

namespace GoussanBlogData.Models.MediaModels;
public class Video
{
    // REQUIRED
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("filename")]
    public string Filename { get; set; }
    [JsonProperty("created")]
    public string Created { get; set; }
    [JsonProperty("modified")]
    public string Updated { get; set; }
    [JsonProperty("state")]
    public string State { get; set; }
    [JsonProperty("userid")]
    public string UserId { get; set; }
    // OPTIONAL
    [JsonProperty("storagepath")]
    public string? StoragePath { get; set; }
    [JsonProperty("blogid")]
    public string? BlogId { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }

}

