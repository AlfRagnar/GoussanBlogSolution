using GoussanBlogData.Models.MediaModels;
using Newtonsoft.Json;

namespace GoussanBlogData.Models.DatabaseModels;
public class User
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("username")]
    public string UserName { get; set; }
    [JsonProperty("password")]
    public string PassWord { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("medias")]
    public Media? Medias { get; set; }
    [JsonProperty("created")]
    public DateTime Created { get; set; }
    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("confirmationcode")]
    public string ConfirmationCode { get; set; }
}
