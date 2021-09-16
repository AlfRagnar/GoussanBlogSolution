
using GoussanBlogData.Models.ChatModels;
using System.Threading.Tasks;

namespace GoussanBlogData.Hubs.Clients;

/// <summary>
/// Client Interface used to define the commands available for the client to call to interact with the Chat Hub
/// </summary>
public interface IChatClient
{
    /// <summary>
    /// Called whenever a message is received
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task ReceiveMessage(ChatMessage message);
}
