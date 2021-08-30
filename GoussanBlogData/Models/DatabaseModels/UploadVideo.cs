using Newtonsoft.Json;

namespace GoussanBlogData.Models.DatabaseModels;
public class UploadVideo
{
    // REQUIRED
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("filename")]
    public string Filename { get; set; }
    [JsonProperty("extension")]
    public string Extension { get; set; }
    [JsonProperty("size")]
    public long Size { get; set; }
    [JsonProperty("created")]
    public string Created { get; set; }
    [JsonProperty("modified")]
    public string Updated { get; set; }
    [JsonProperty("state")]
    public string State { get; set; }
    [JsonProperty("userid")]
    public string UserId { get; set; }
    [JsonProperty("locator")]
    public string Locator { get; set; }
    [JsonProperty("outputasset")]
    public string OutputAsset { get; set; }
    // OPTIONAL
    [JsonProperty("streamingpaths")]
    public IList<string> StreamingPaths { get; set; }
    [JsonProperty("blogid")]
    public string? BlogId { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }

}

