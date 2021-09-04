using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.DatabaseModels;
public class UploadVideo
{
    // REQUIRED
    [Required]
    [JsonProperty("id")]
    public string Id { get; set; }
    [Required]
    [JsonProperty("type")]
    public string Type { get; set; }
    [Required]
    [JsonProperty("title")]
    public string Title { get; set; }
    [Required]
    [JsonProperty("description")]
    public string Description { get; set; }
    // OPTIONAL
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
    [JsonProperty("streamingpaths")]
    public IList<string> StreamingPaths { get; set; }
    [JsonProperty("thumbnailurl")]
    public string ThumbnailUrl { get; set; }

    [JsonProperty("blogid")]
    public string BlogId { get; set; }


}

