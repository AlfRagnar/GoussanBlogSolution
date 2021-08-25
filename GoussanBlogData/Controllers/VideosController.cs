using GoussanBlogData.Models.MediaModels;
using GoussanBlogData.Services;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GoussanBlogData.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VideosController : ControllerBase
{
    private readonly ILogger<VideosController> _logger;
    private readonly ICosmosDbService cosmosDb;

    public VideosController(ICosmosDbService cosmosDb, ILogger<VideosController> logger)
    {
        this.cosmosDb = cosmosDb;
        _logger = logger;
    }

    // GET /videos
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return Ok(await cosmosDb.GetMultipleVideosAsync("SELECT * FROM c"));
    }

    // GET /videos/{ID}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await cosmosDb.GetVideoAsync(id));
    }

    // POST /videos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VideoCreateModel video)
    {
        Video newVideo = new()
        {
            Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
            Filename = video.Filename,
            Created = $"{DateTime.UtcNow.ToShortDateString()} UTC",
            Updated = $"{DateTime.UtcNow.ToUniversalTime()} UTC",
            State = "Not Set",
            UserId = video.UserId,
            BlogId = video.BlogId,
            Description = video.Description,
            Title = video.Title
        };
        await cosmosDb.AddVideo(newVideo);
        return CreatedAtAction(nameof(Get), new { Id = newVideo.Id }, newVideo);
    }

    // PUT /videos/{ID}
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] Video video)
    {
        try
        {
            await cosmosDb.UpdateVideoAsync(video.Id, video);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    // DELETE /videos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await cosmosDb.DeleteVideoAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }

    }
}
