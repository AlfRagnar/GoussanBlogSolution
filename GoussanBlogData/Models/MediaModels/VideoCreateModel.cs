using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.MediaModels;

/// <summary>
/// Model used to ensure the request has the proper properties before the controller attempts to create a object and send it to Cosmos DB
/// </summary>
public class VideoCreateModel
{
    // REQUIRED

    /// <summary>
    /// Require that a Title property is set
    /// </summary>
    [Required]
    [JsonProperty("title")]
    public string Title { get; set; }

    /// <summary>
    /// Require that the Description property is set
    /// </summary>
    [Required]
    [JsonProperty("description")]
    public string Description { get; set; }
    /// <summary>
    /// Require there to be a file present in the request
    /// </summary>
    [JsonProperty("uploadfile")]
    public IFormFile File { get; set; }

    // OPTIONAL
    /// <summary>
    /// Optionally include a Blog ID that is used to tie a video to a Blog Post
    /// </summary>
    [JsonProperty("blogid")]
    public string BlogId { get; set; }



}

