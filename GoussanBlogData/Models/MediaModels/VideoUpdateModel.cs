using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.MediaModels;

/// <summary>
/// Model used to handle validation for Update Requests on Videos
/// </summary>
public class VideoUpdateModel
{
    // REQUIRED
    /// <summary>
    /// Require that an ID is supplied when a Video is requested to be updated
    /// </summary>
    [Required]
    [JsonProperty("id")]
    public string Id { get; set; }
    /// <summary>
    /// Check if there is an User ID supplied in case of Update on Behalf of someone
    /// </summary>
    [JsonProperty("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// Check if a filename is supplied
    /// </summary>
    [JsonProperty("filename")]
    public string Filename { get; set; }
    /// <summary>
    /// Property to store file size
    /// </summary>
    [JsonProperty("filesize")]
    public string Filesize { get; set; }
    /// <summary>
    /// Property to store Content Type
    /// </summary>
    [JsonProperty("contenttype")]
    public string Contenttype { get; set; }

    /// <summary>
    /// Property to store Blog ID if it is tied to a blog
    /// </summary>
    [JsonProperty("blogid")]
    public string BlogId { get; set; }
    /// <summary>
    /// Property to store title if it is being changed
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; }
    /// <summary>
    /// Property to store Description if it is being changed
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

}

