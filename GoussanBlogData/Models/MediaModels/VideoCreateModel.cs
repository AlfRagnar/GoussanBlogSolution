using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.MediaModels;
public class VideoCreateModel
{
    // REQUIRED
    [Required]
    [JsonProperty("title")]
    public string Title { get; set; }
    [Required]
    [JsonProperty("description")]
    public string Description { get; set; }
    [Required]
    [JsonProperty("uploadfile")]
    public IFormFile File { get; set; }
    [Required]
    [JsonProperty("token")]
    public string Token { get; set; }

    // OPTIONAL
    [JsonProperty("userid")]
    public string? UserId { get; set; }
    [JsonProperty("blogid")]
    public string? BlogId { get; set; }



}

