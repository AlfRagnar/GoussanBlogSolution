
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.ChatModels;
/// <summary>
/// Model used by ChatHub to send message
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Username of the user sending the message
    /// </summary>
    [Required]
    public string User { get; set; }
    /// <summary>
    /// Content of the message
    /// </summary>
    [Required]
    public string Message { get; set; }
}
