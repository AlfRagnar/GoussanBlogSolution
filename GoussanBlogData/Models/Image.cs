
using System.Text.Json.Serialization;

namespace GoussanBlogData.Models;
public class Image
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("filename")]
    public string FileName { get; set; }
    [JsonPropertyName("created")]
    public string Created { get; set; }
    [JsonPropertyName("modified")]
    public string Modified { get; set; }
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    [JsonPropertyName("blogid")]
    public string BlogId { get; set; }
    [JsonPropertyName("storagepath")]
    public string StoragePath { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

}
