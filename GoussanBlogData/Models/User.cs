
using Newtonsoft.Json;

namespace GoussanBlogData.Models;
public class User
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonProperty("created")]
    public string? Created { get; set; }
    [JsonProperty("username")]
    public string UserName { get; set; }
    [JsonProperty("password")]
    public string PassWord { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("medias")]
    public Media? Medias { get; set; }
}
