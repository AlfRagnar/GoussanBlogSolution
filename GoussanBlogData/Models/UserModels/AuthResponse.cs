
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.UserModels;
public class AuthResponse
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string JwtToken { get; set; }
}
