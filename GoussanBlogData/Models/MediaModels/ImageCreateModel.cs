using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.MediaModels;

/// <summary>
/// Image Model used to accept requests to create a new Image
/// </summary>
public class ImageCreateModel
{
    // REQUIRED
    /// <summary>
    /// Require the Title property to be filled out or set
    /// </summary>
    [Required]
    [JsonProperty("title")]
    public string Title { get; set; }
    /// <summary>
    /// Require the Description field to be filled out or set
    /// </summary>
    [Required]
    [JsonProperty("description")]
    public string Description { get; set; }
    /// <summary>
    /// Require there to be a file present
    /// </summary>
    [Required]
    [JsonProperty("uploadfile")]
    public IFormFile File { get; set; }
    

    // OPTIONAL

    /// <summary>
    /// Optional property in case the image is tied to a blog post
    /// </summary>
    [JsonProperty("blogid")]
    public string BlogId { get; set; }

}
