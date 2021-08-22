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
        return Ok(await cosmosDb.GetMultipleAsync("SELECT * FROM c"));
    }

    // GET /videos/{ID}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await cosmosDb.GetAsync(id));
    }

    // POST /videos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Video video)
    {
        video.id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        await cosmosDb.AddAsync(video);
        return CreatedAtAction(nameof(Get), new { id = video.id }, video);
    }

    // PUT /videos/{ID}
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] Video video)
    {
        await cosmosDb.UpdateAsync(video.id, video);
        return NoContent();
    }

    // DELETE /videos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await cosmosDb.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }

    }
}
