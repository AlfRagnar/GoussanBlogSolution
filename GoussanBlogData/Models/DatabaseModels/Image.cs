
using Newtonsoft.Json;

namespace GoussanBlogData.Models.DatabaseModels;
public class Image
{

    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("filename")]
    public string FileName { get; set; }
    [JsonProperty("created")]
    public string Created { get; set; }
    [JsonProperty("lastmodified")]
    public string LastModified { get; set; }
    [JsonProperty("userid")]
    public string UserId { get; set; }
    [JsonProperty("blogid")]
    public string BlogId { get; set; }
    [JsonProperty("storagepath")]
    public string StoragePath { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

}
