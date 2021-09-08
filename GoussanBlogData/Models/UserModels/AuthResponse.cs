namespace GoussanBlogData.Models.UserModels;

/// <summary>
/// Authentication Response Model
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// ID of the User authenticating
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Username of the user authenticating
    /// </summary>
    public string Username { get; set; }
    /// <summary>
    /// Email of the user authenticating
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// JWT Token of the user authenticating
    /// </summary>
    public string JwtToken { get; set; }
}
