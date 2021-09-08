using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.DatabaseModels;
/// <summary>
/// Model of the Blog object stored in the database
/// </summary>
public class BlogPost
{
    /// <summary>
    /// Need to set an ID for the blog post, for indexing and ensuring it is unique
    /// </summary>
    [Required]
    [JsonProperty("id")]
    public string Id { get; set; }
    /// <summary>
    /// Need to define the type to easier categorise it, since it is stored in one container
    /// </summary>
    [Required]
    [JsonProperty("type")]
    public string Type { get; set; }
    /// <summary>
    /// Store the user ID of who created the blog post
    /// </summary>
    [Required]
    [JsonProperty("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// Set the title of the Blog Post
    /// </summary>
    [Required]
    [JsonProperty("title")]
    public string Title { get; set; }
    /// <summary>
    /// Need to define the content of the blog post along with formatting
    /// </summary>
    [Required]
    [JsonProperty("content")]
    public string Content { get; set; }
    /// <summary>
    /// Any videos tied to the blog post?
    /// </summary>
    [JsonProperty("videos")]
    public List<UploadVideo> Videos { get; set; }
    /// <summary>
    /// Any Images tied to the blog post?
    /// </summary>
    [JsonProperty("images")]
    public List<Image> Images { get; set; }
}
