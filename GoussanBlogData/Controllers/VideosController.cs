using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.MediaModels;
using GoussanBlogData.Services;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoussanBlogData.Controllers;
/// <summary>
/// Here Interaction with the Video Endpoint is handled and defined
/// </summary>
[Produces("application/json")]
[Authorize]
[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
    private readonly ILogger<VideosController> _logger;
    private readonly ICosmosDbService cosmosDb;
    private readonly IGoussanMediaService mediaService;
    private readonly IBlobStorageService blobStorage;
    private readonly IJwtUtils jwtUtils;
    /// <summary>
    /// Constructor function required to initialize services in use by the Controller
    /// </summary>
    /// <param name="cosmosDb"></param>
    /// <param name="logger"></param>
    /// <param name="mediaService"></param>
    /// <param name="jwtUtils"></param>
    /// <param name="blobStorage"></param>
    public VideosController(ICosmosDbService cosmosDb, ILogger<VideosController> logger, IGoussanMediaService mediaService, IJwtUtils jwtUtils, IBlobStorageService blobStorage)
    {
        this.cosmosDb = cosmosDb;
        this.mediaService = mediaService;
        _logger = logger;
        this.jwtUtils = jwtUtils;
        this.blobStorage = blobStorage;
    }


    /// <summary>
    /// Response with a JSON object containing a list of videos that has finished Encoding
    /// </summary>
    /// <returns></returns>
    // GET /videos
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> List()
    {
        try
        {
            IEnumerable<UploadVideo> videoEnum = await cosmosDb.GetVideoList();
            var videoList = videoEnum.ToList();
            foreach (var video in new List<UploadVideo>(videoList))
            {
                try
                {
                    if (video.StreamingPaths == null)
                    {
                        video.StreamingPaths = await mediaService.GetStreamingURL(video.Locator);
                    }
                    if (!video.StreamingPaths.Any())
                    {
                        //video.State = "Error";
                        videoList.Remove(video);
                    }
                    await cosmosDb.UpdateVideoAsync(video.Id, video).ConfigureAwait(false);

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
    /// Respond with a list of ALL Video Objects in Database
    /// </summary>
    /// <returns>List of Videos</returns>
    // GET /videos/all
    [AllowAnonymous]
    [HttpGet("all")]
    public async Task<IActionResult> AllVideos()
    {
        try
        {
            IEnumerable<UploadVideo> videoEnum = await cosmosDb.GetAllVideosAsync();
            var videoList = videoEnum.ToList();
            if(videoList == null || videoList.Count == 0)
            {
                return NotFound();
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
    /// <response code="200">Returns Video Object with matching input ID</response>
    /// <response code="400">If the input fails validation</response>
    /// <response code="404">If can't find a file matching the ID</response>

    // GET /videos/{ID}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var res = await cosmosDb.GetVideoAsync(id);
            if(res == null)
            {
                return NotFound(id);
            }
            return Ok(res);
        }
        catch (Exception)
        {
            return BadRequest(id);
        }
    }

    /// <summary>
    /// Retrieves a list of Videos that have not finished Encoding
    /// </summary>
    /// <returns>List of Videos</returns>
    // GET /videos/halted
    [AllowAnonymous]
    [HttpGet("halted")]
    public async Task<IActionResult> Halted()
    {
        try
        {
            var res = await cosmosDb.GetNotFinishedVideosAsync();
            if (res == null)
            {
                return NotFound();
            }
            var list = res.ToList();
            return Ok(list);
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
    /// <response code="201">If Video is created and returns ID of the new Video Object</response>
    /// <response code="400">If the input file fails validation</response>

    // POST /videos
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] VideoCreateModel createVideoReq)
    {
        try
        {
            User user = (User)HttpContext.Items["User"];

            if (createVideoReq.File != null)
            {

                UploadVideo newVideo = new()
                {
                    Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
                    Filename = createVideoReq.File.Name,
                    Extension = createVideoReq.File.ContentType,
                    Size = createVideoReq.File.Length,
                    Created = DateTime.UtcNow.ToShortDateString(),
                    Updated = DateTime.UtcNow.ToString(),
                    State = "Not Set",
                    UserId = user!.Id,
                    BlogId = createVideoReq.BlogId,
                    Description = createVideoReq.Description,
                    Title = createVideoReq.Title,
                    Type = "Video"
                };
                var res = await mediaService.CreateAsset(createVideoReq.File, newVideo);
                if (res != null)
                {
                    await cosmosDb.AddVideo(res);
                    return Created(res.Id, res);
                }
                return BadRequest(createVideoReq);
            }
            else
            {
                UploadVideo newVideoRef = new()
                {
                    Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
                    Created = DateTime.UtcNow.ToShortDateString(),
                    Updated = DateTime.UtcNow.ToString(),
                    State = "Not Set",
                    UserId = user!.Id,
                    BlogId = createVideoReq.BlogId,
                    Description = createVideoReq.Description,
                    Title = createVideoReq.Title,
                    Type = "Video"
                };

                var res = await mediaService.CreateAsset(newVideoRef).ConfigureAwait(true);
                var containerClient = await blobStorage.GetContainer("asset-" + res.Assetid);
                var sas = blobStorage.GetServiceSasUriForContainer(containerClient);
                var SASToken = sas.Query.TrimStart('?');

                res.Sas = SASToken;
                res.SASUri = sas.AbsoluteUri;
                res.accountUrl = $"{containerClient.Uri.Scheme}://{containerClient.Uri.Host}";


                if (res != null)
                {
                    await cosmosDb.AddVideo(res);
                    return Created(res.Id, res);
                }
                return BadRequest(createVideoReq);
            }
        }
        catch (Exception)
        {
            return BadRequest(createVideoReq);
        }
    }

    /// <summary>
    /// Request to Update a Video, usually called once a large video file has finished uploading to Azure Storage from the Client App
    /// </summary>
    /// <param name="videoUpdate"></param>
    /// <response code="200">If Task is successfully started</response>
    /// <response code="404">If Video cannot be found</response>
    /// <response code="400">If some of the required fields are not filled out correctly</response>

    // POST: /videos/update
    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] VideoUpdateModel videoUpdate)
    {
        var orgVideo = await cosmosDb.GetVideoAsync(videoUpdate.Id);
        if (orgVideo == null)
        {
            return NotFound("Video not found with ID: " + videoUpdate.Id);
        }
        else
        {
            try
            {
                var res = await mediaService.SubmitJobAsync(orgVideo.Id, orgVideo.OutputAsset);
                if(res == null)
                {
                    return BadRequest(videoUpdate.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(orgVideo.Id);
            }
            if (!string.IsNullOrEmpty(videoUpdate.Filename))
            {
                orgVideo.Filename = videoUpdate.Filename;
            }
            if (!string.IsNullOrEmpty(videoUpdate.Filesize))
            {
                orgVideo.Size = Convert.ToInt32(videoUpdate.Filesize);
            }
            if (!string.IsNullOrEmpty(videoUpdate.Contenttype))
            {
                orgVideo.Extension = videoUpdate.Contenttype;
            }
            await cosmosDb.UpdateVideoAsync(orgVideo.Id, orgVideo);
            return Ok(orgVideo);
        }
    }


    /// <summary>
    /// Request to EDIT a specific Video Object stored in Database.
    /// This is the Data stored in the database, NOT the file object
    /// </summary>
    /// <param name="video"></param>
    /// <response code="202">ID of Video Requested to be updated</response>
    /// <response code="404">If Video with given ID is not found</response>
    /// <returns>ID of the video you requested to be updated</returns>
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
            return NotFound(video.Id);
        }
    }

    /// <summary>
    /// Request to Remove a video from the Database.
    /// THIS IS NOT THE FILE OBJECT
    /// This is just the Video Object stored in Database
    /// </summary>
    /// <param name="id"></param>
    /// <response code="202">If video is set to be deleted</response>
    /// <response code="404">If Video with given ID is not found</response>
    /// <returns>ID of video being deleted</returns>

    // DELETE /videos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var res = await cosmosDb.DeleteVideoAsync(id);
            if(res == null)
            {
                return BadRequest(id);
            }
            return Accepted(id);
        }
        catch (Exception)
        {
            return NotFound(id);
        }

    }
}
