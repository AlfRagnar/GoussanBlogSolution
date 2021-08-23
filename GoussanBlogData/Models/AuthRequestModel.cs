
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models;
public class AuthRequestModel
{
    [Required]
    [JsonProperty("username")]
    public string Username { get; set; }
    [Required]
    [JsonProperty("password")]
    public string Password { get; set; }
}
