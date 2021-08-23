
using Newtonsoft.Json;

namespace GoussanBlogData.Models;
public class ShadowUser
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("salt")]
    public string Salt { get; set; }
}
