using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.MediaModels;
using GoussanBlogData.Services;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GoussanBlogData.Controllers;
[Produces("application/json")]
[Authorize]
[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
    private readonly ILogger<VideosController> _logger;
    private readonly ICosmosDbService cosmosDb;
    private readonly IGoussanMediaService mediaService;
    private readonly IJwtUtils jwtUtils;

    public VideosController(ICosmosDbService cosmosDb, ILogger<VideosController> logger, IGoussanMediaService mediaService, IJwtUtils jwtUtils)
    {
        this.cosmosDb = cosmosDb;
        this.mediaService = mediaService;
        _logger = logger;
        this.jwtUtils = jwtUtils;
    }


    /// <summary>
    /// Response with a JSON object containing a list of currently available videos
    /// </summary>
    /// <returns></returns>
    // GET /videos
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> List()
    {
        try
        {
            var videoList = await cosmosDb.GetVideoList();
            foreach (var video in videoList)
            {
                try
                {
                    if (video.StreamingPaths == null)
                    {
                        video.StreamingPaths = await mediaService.GetStreamingURL(video.Locator);
                        await cosmosDb.UpdateVideoAsync(video.Id, video).ConfigureAwait(false);
                    }
                }
                catch (Exception)
                {
                    video.StreamingPaths = null!;
                }
            }

            return Ok(videoList);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Get a Specific Video Object from Cosmos DB using the Video ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    // GET /videos/{ID}
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

    /// <summary>
    /// POST request to API to upload a video to Azure Media Services and Add it to Cosmos DB.
    /// This will Auto Populate to the website once the Video Object has finished encoding and a Streaming URL has become available.
    /// All this is done in the backend using Azure Media Services, Azure Functions and Cosmos DB
    /// </summary>
    /// <param name="createVideoReq"></param>
    /// <returns></returns>

    // POST /videos
    [HttpPost]
    [RequestSizeLimit(300000000)]
    public async Task<IActionResult> Create([FromForm]VideoCreateModel createVideoReq)
    {
        try
        {
            var userId = jwtUtils.ValidateToken(createVideoReq.Token);
            UploadVideo newVideo = new()
            {
                Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
                Filename = createVideoReq.File.Name,
                Extension = createVideoReq.File.ContentType,
                Size = createVideoReq.File.Length,
                Created = DateTime.UtcNow.ToShortDateString(),
                Updated = DateTime.UtcNow.ToString(),
                State = "Not Set",
                UserId = userId!,
                BlogId = createVideoReq.BlogId,
                Description = createVideoReq.Description,
                Title = createVideoReq.Title,
                Type = "Video"
            };
            var res = await mediaService.CreateAsset(createVideoReq.File, newVideo);
            if (res != null)
            {
                await cosmosDb.AddVideo(res);
                return CreatedAtAction(nameof(Create), new { res.Id }, res);
            }
            return BadRequest();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }


    /// <summary>
    /// Request to EDIT a specific Video Object stored in Database.
    /// This is the Data stored in the database, NOT the file object
    /// </summary>
    /// <param name="video"></param>
    /// <returns>Accepted 202 Response with the ID of the video you requested to be changed</returns>
    // PUT /videos/{ID}
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] VideoUpdateModel video)
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

    /// <summary>
    /// Request to Remove a video from the Database.
    /// THIS IS NOT THE FILE OBJECT
    /// This is just the Video Object stored in Database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    // DELETE /videos/{id}
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
