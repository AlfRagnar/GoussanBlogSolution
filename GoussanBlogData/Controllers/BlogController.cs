using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.MediaModels;
using GoussanBlogData.Services;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoussanBlogData.Controllers;
/// <summary>
/// Controlled used to handle Blog CRUD actions
/// </summary>
[Authorize]
[Route("[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly ICosmosDbService cosmosDb;
    private readonly IGoussanMediaService mediaService;
    private readonly IBlobStorageService storageService;

    /// <summary>
    /// Constructor function to initialize usage of Cosmos DB Service, Azure Media Services and Azure Blog Storage Service
    /// </summary>
    /// <param name="cosmosDb"></param>
    /// <param name="mediaService"></param>
    /// <param name="storageService"></param>
    public BlogController(ICosmosDbService cosmosDb, IGoussanMediaService mediaService, IBlobStorageService storageService)
    {
        this.cosmosDb = cosmosDb;
        this.mediaService = mediaService;
        this.storageService = storageService;
    }




    // GET: blog/
    /// <summary>
    /// Api call to get blog posts available in the database
    /// </summary>
    /// <returns>List of Blog Posts</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var list = await cosmosDb.GetBlogs();
        return Ok(list);
    }

    // GET blog/5
    /// <summary>
    /// Tries to get a specific blog based on ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Blog Post</returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var list = await cosmosDb.GetBlogs(id);
        if (list.Any())
        {
            return Ok(list);
        }
        else
        {
            return NotFound(id);
        }
    }

    // DELETE blog/5
    /// <summary>
    /// Request to delete a blog object stored in the database
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">If Delete request is successful</response>
    /// <response code="404">If Blog Object is not Found</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var res = await cosmosDb.DeleteBlog(id);
        if (res != null)
        {
            return Ok(res);
        }
        else
        {
            return NotFound(res);
        }
    }

    // POST <BlogController>
    /// <summary>
    /// Tries to create a new blog post with the supplied information from the request
    /// </summary>
    /// <param name="post"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BlogPostCreateModel post)
    {
        try
        {
            User user = (User)HttpContext.Items["User"];
            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return Unauthorized("Failed to Authenticate User, please try again later");
            }
            // Create new Blog Post object that will be stored in the database
            BlogPost newPost = new()
            {
                Title = post.Title,
                Content = post.Content,
                Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
                Type = "Blog",
                UserId = user.Id
            };
            // Check if there is any videos attached and if there is, attach them to the Blog Post Object
            if (post.Videos != null)
            {
                var video = post.Videos;
                UploadVideo newVideo = new()
                {
                    Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
                    Filename = video.File.Name,
                    Extension = video.File.ContentType,
                    Size = video.File.Length,
                    Created = DateTime.UtcNow.ToShortDateString(),
                    Updated = DateTime.UtcNow.ToString(),
                    State = "Not Set",
                    UserId = user.Id,
                    BlogId = newPost.Id,
                    Description = video.Description,
                    Title = video.Title,
                    Type = "Video"
                };
                var res = await mediaService.CreateAsset(video.File, newVideo);
            }
            // Check if the Blog Creation Request contains any Images
            if (post.Images != null)
            {
                var image = post.Images;
                Image newImage = new()
                {
                    Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
                    Title = image.Title,
                    Description = image.Description,
                    UserId = user.Id,
                    BlogId = newPost.Id,
                    Created = DateTime.UtcNow.ToShortDateString(),
                    LastModified = DateTime.UtcNow.ToString(),
                    FileName = image.File.FileName,
                    ContentType = image.File.ContentType,
                    Type = "Image",
                };
                var res = await storageService.UploadImage(image.File, newImage.Id);
            }

            var response = await cosmosDb.CreateBlogPost(newPost);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return CreatedAtAction(nameof(Get), new { id = newPost.Id }, newPost);
            }
            else
            {
                return BadRequest(response);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest();
        }

    }
}
