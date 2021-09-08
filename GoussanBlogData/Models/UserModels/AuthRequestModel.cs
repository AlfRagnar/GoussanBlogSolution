
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.UserModels;
/// <summary>
/// Authentication Request Model used to handle Authentications
/// </summary>
public class AuthRequestModel
{
    /// <summary>
    /// Require that the Username field is include
    /// </summary>
    [Required]
    [JsonProperty("username")]
    public string Username { get; set; }
    /// <summary>
    /// Require that the password is included
    /// </summary>
    [Required]
    [JsonProperty("password")]
    public string Password { get; set; }
}
