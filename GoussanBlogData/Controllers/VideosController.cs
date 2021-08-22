using GoussanBlogData.Models;
using GoussanBlogData.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GoussanBlogData.Controllers;
[ApiController]
[Route("[controller]")]
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
    public async Task<IActionResult> Create([FromBody] Video video)
    {
        video.id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        await cosmosDb.AddVideo(video);
        return CreatedAtAction(nameof(Get), new { video.id }, video);
    }

    // PUT /videos/{ID}
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] Video video)
    {
        try
        {
            await cosmosDb.UpdateVideoAsync(video.id!, video);
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
