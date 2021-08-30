﻿using Newtonsoft.Json;

namespace GoussanBlogData.Models.MediaModels;
public class VideoCreateModel
{
    // REQUIRED
    [JsonProperty("filename")]
    public string Filename { get; set; }

    [JsonProperty("userid")]
    public string UserId { get; set; }
    [JsonProperty("uploadfile")]
    public IFormFile File { get; set; }

    // OPTIONAL
    [JsonProperty("blogid")]
    public string? BlogId { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }

}

