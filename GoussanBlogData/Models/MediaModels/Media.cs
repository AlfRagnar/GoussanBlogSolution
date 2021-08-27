using GoussanBlogData.Models.DatabaseModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoussanBlogData.Models.MediaModels;

public class Media
{
    [JsonProperty("userid")]
    public string UserId { get; set; }
    [JsonProperty("images")]
    public List<Image> Images { get; set; }
    [JsonProperty("videos")]
    public List<UploadVideo> Video { get; set; }
    [JsonProperty("blogposts")]
    public List<BlogPost> BlogPosts { get; set; }
}
