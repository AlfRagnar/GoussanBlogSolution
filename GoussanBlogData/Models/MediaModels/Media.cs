using GoussanBlogData.Models.DatabaseModels;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.MediaModels;
/// <summary>
/// This is the model used to handle requests to get a list of what a used has created/stored in the App
/// </summary>
public class Media
{
    /// <summary>
    /// Require that a user ID is supplied for when the call is made, this is to get the user content of the user you wish to see
    /// </summary>
    [Required]
    [JsonProperty("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// List of Images made by the user
    /// </summary>
    [JsonProperty("images")]
    public List<Image> Images { get; set; }
    /// <summary>
    /// List of videos made by the user
    /// </summary>
    [JsonProperty("videos")]
    public List<UploadVideo> Video { get; set; }
    /// <summary>
    /// List of blog posts made by the user
    /// </summary>
    [JsonProperty("blogposts")]
    public List<BlogPost> BlogPosts { get; set; }
}
