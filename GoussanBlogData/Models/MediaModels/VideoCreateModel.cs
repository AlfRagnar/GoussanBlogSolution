using Newtonsoft.Json;

namespace GoussanBlogData.Models.MediaModels;
public class VideoCreateModel
{
    // REQUIRED
    [JsonProperty("filename")]
    public string Filename { get; set; }
    [JsonProperty("filesize")]
    public int FileSize { get; set; }
    [JsonProperty("extension")]
    public string Extension { get; set; }
    [JsonProperty("userid")]
    public string UserId { get; set; }

    // OPTIONAL
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("blogid")]
    public string? BlogId { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }

}

