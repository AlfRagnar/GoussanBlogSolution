
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.MediaModels;

/// <summary>
/// Blog Post Creation Model to handle incoming User Requests and validate that it has all the required properties
/// before controller tries to action the result
/// </summary>
public class BlogPostCreateModel
{
    /// <summary>
    /// Require that the incoming request has a title set for the post
    /// </summary>
    [Required]
    [JsonProperty("title")]
    public string Title { get; set; }
    /// <summary>
    /// Require that the incoming request has some data in the content field
    /// </summary>
    [Required]
    [JsonProperty("content")]
    public string Content { get; set; }
    /// <summary>
    /// Let them optionally attach Videos to the request creating a Blog Post
    /// </summary>
    [JsonProperty("videos")]
    public List<VideoCreateModel> Videos { get; set; }
    /// <summary>
    /// Let them optionally attach Images to the request when creating a Blog Post
    /// </summary>
    [JsonProperty("images")]
    public List<ImageCreateModel> Images { get; set; }
}
