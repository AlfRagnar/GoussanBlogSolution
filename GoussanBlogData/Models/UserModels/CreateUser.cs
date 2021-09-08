
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.UserModels;
/// <summary>
/// User Creation Model with the fields required to be filled out before creation requests are handled
/// </summary>
public class CreateUser
{
    /// <summary>
    /// Require that the username field is filled
    /// </summary>
    [Required]
    [JsonProperty("username")]
    public string Username { get; set; }
    /// <summary>
    /// Require that the email field is filled
    /// </summary>
    [Required]
    [JsonProperty("email")]
    public string Email { get; set; }
    /// <summary>
    /// Require that the password field is filled
    /// </summary>
    [Required]
    [JsonProperty("password")]
    public string Password { get; set; }
}
