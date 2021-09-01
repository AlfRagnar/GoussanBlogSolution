using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.MediaModels;
public class VideoUpdateModel
{
    // REQUIRED
    [Required]
    [JsonProperty("id")]
    public string Id { get; set; }
    [Required]
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

