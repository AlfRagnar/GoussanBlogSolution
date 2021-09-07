
using GoussanBlogData.Hubs.Clients;
using GoussanBlogData.Models.ChatModels;
using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Services;
using Microsoft.AspNetCore.SignalR;

namespace GoussanBlogData.Hubs;

/// <summary>
/// This is the class definition of ChatHub that is inheriting for Azure SignalR Hub.
/// This is where we will be handling the Real-Time Chatting through usage of Azure SignalR functionality
/// The way this will work is it will have a server-client interaction.
/// PS: Hubs are transient, don't store state in a property on the hub class
/// PS: Use Await when calling async methods that depend on the hub staying alive
/// </summary>
public class ChatHub : Hub<IChatClient>
{
    private readonly ICosmosDbService cosmosDb;
    /// <summary>
    /// Constructor function to initialize Cosmos Service
    /// </summary>
    /// <param name="cosmosDb"></param>
    public ChatHub(ICosmosDbService cosmosDb)
    {
        this.cosmosDb = cosmosDb;
    }
    /// <summary>
    /// Command called for when a user sends a Message
    /// </summary>
    /// <param name="msgObj"></param>
    /// <returns></returns>
    public async Task SendMessage(ChatMessage msgObj)
    {
        LoggedMessage logMessage = new()
        {
            User = msgObj.User,
            Id = Guid.NewGuid().ToString(),
            Creationtime = DateTime.UtcNow.ToString(),
            Message = msgObj.Message
        };
        await cosmosDb.AddMessage(logMessage);
        await Clients.All.ReceiveMessage(msgObj);
    }
}
