
using GoussanBlogData.Models.ChatModels;
using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Services;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoussanBlogData.Controllers;

/// <summary>
/// Controller that handles API requests like trying to get Chat History etc
/// </summary>
[Authorize]
[Route("[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly ICosmosDbService cosmosDb;

    /// <summary>
    /// Constructor function to initialize Cosmos DB Service
    /// </summary>
    /// <param name="cosmosDb"></param>
    public ChatController( ICosmosDbService cosmosDb)
    {
        this.cosmosDb = cosmosDb;
    }


    // GET: /chat/history

    /// <summary>
    /// Returns the last 100 chat messages
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("history")]
    public async Task<IActionResult> History()
    {
        List<LoggedMessage> list = await cosmosDb.GetChatHistory();
        return Ok(list);
    }

}
