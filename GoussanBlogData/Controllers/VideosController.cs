using GoussanBlogData.Models.MediaModels;
using GoussanBlogData.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GoussanBlogData.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VideosController : ControllerBase
{
    private readonly ILogger<VideosController> _logger;
    private readonly ICosmosDbService cosmosDb;
    private readonly IGoussanMediaService mediaService;

    public VideosController(ICosmosDbService cosmosDb, ILogger<VideosController> logger, IGoussanMediaService mediaService)
    {
        this.cosmosDb = cosmosDb;
        this.mediaService = mediaService;
        _logger = logger;
    }

    // GET /api/videos
    [HttpGet]
    public async Task<IActionResult> List()
    {
        try
        {
            return Ok(await cosmosDb.GetVideoList());
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    // GET /api/videos/{ID}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            return Ok(await cosmosDb.GetVideoAsync(id));
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    // POST /api/videos
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] VideoCreateModel video)
    {
        try
        {
            UploadVideo newVideo = new()
            {
                Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
                Filename = video.Filename,
                Created = $"{DateTime.UtcNow.ToShortDateString()} UTC",
                Updated = $"{DateTime.UtcNow.ToUniversalTime()} UTC",
                State = "Not Set",
                UserId = video.UserId,
                BlogId = video.BlogId,
                Description = video.Description,
                Title = video.Title,
                Type = "Video"
            };
            var res = await mediaService.CreateAsset(video.File, newVideo);
            if (res != null)
            {
                await cosmosDb.AddVideo(newVideo);
                return CreatedAtAction(nameof(Create), new { Id = newVideo.Id }, newVideo);
            }
            return BadRequest();
        }
        catch (Exception)
        {
            return BadRequest();
        }

    }

    // PUT /api/videos/{ID}
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] UploadVideo video)
    {
        try
        {
            await cosmosDb.UpdateVideoAsync(video.Id, video);
            return AcceptedAtAction(nameof(Edit), video.Id);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    // DELETE /api/videos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await cosmosDb.DeleteVideoAsync(id);
            return AcceptedAtAction(nameof(Delete), id);
        }
        catch (Exception)
        {
            return NotFound();
        }

    }
}
