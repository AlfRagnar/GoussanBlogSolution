using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.DatabaseModels;
/// <summary>
/// Model used in Goussanjarga Database
/// </summary>
public class UploadVideo
{
    /// <summary>
    /// ID is required before the new Object can be manipulated
    /// </summary>
    [Required]
    [JsonProperty("id")]
    public string Id { get; set; }
    /// <summary>
    /// A type has to be set
    /// Example: "Not Set, Videos, Images" etc.
    /// </summary>
    [Required]
    [JsonProperty("type")]
    public string Type { get; set; }
    /// <summary>
    /// A title is required to define what this Object is
    /// </summary>
    [Required]
    [JsonProperty("title")]
    public string Title { get; set; }
    /// <summary>
    /// Description of what this Object contains
    /// </summary>
    [Required]
    [JsonProperty("description")]
    public string Description { get; set; }
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
    /// <summary>
    /// Asset ID of the Video Object
    /// </summary>
    [JsonProperty("assetid")]
    public string Assetid { get; set; }
    /// <summary>
    /// Output Asset the Encoded File is put into by Azure Media Service
    /// </summary>

    [JsonProperty("outputasset")]
    public string OutputAsset { get; set; }
    [JsonProperty("streamingpaths")]
    public IList<string> StreamingPaths { get; set; }
    [JsonProperty("thumbnailurl")]
    public string ThumbnailUrl { get; set; }

    [JsonProperty("blogid")]
    public string BlogId { get; set; }
    [JsonProperty("sas")]
    public string Sas { get; set; }
    public string SASUri { get; internal set; }
    public string accountUrl { get; internal set; }
}

