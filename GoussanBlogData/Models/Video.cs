using System.Text.Json.Serialization;

namespace GoussanBlogData.Models;
public class Video
{
    [JsonPropertyName("id")]
    public string? id { get; set; }

    [JsonPropertyName("outputAsset")]
    public string? OutputAsset { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("filename")]
    public string? FileName { get; set; }

    [JsonPropertyName("bloburi")]
    public string? BlobUri { get; set; }

    [JsonPropertyName("locator")]
    public string? Locator { get; set; }

    [JsonPropertyName("streamingurl")]
    public string? StreamingUrl { get; set; }

    [JsonPropertyName("created")]
    public string? Created { get; set; }

    [JsonPropertyName("lastmodified")]
    public string? LastModified { get; set; }

    [JsonPropertyName("filesize")]
    public int? FileSize { get; set; }

    [JsonPropertyName("extension")]
    public string? Extension { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

}

