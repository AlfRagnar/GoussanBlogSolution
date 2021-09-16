using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.MediaModels;
using GoussanBlogData.Services;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoussanBlogData.Controllers;

/// <summary>
/// This is where all the functions and endpoints are defined for the /Image endpoint
/// </summary>
[Authorize]
[Route("[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IBlobStorageService storageService;
    private readonly ICosmosDbService cosmosDb;

    /// <summary>
    /// Constructor to initialize the services in use by the controller
    /// </summary>
    /// <param name="storageService"></param>
    /// <param name="cosmosDb"></param>
    public ImageController(IBlobStorageService storageService, ICosmosDbService cosmosDb)
    {
        this.storageService = storageService;
        this.cosmosDb = cosmosDb;
    }


    // GET: Image/
    /// <summary>
    /// Gets a list of Images
    /// </summary>
    /// <returns>List of Images</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var list = await cosmosDb.GetImageList();
        return Ok(list);
    }


    /// <summary>
    /// Attempt to Upload image file received to Azure Storage and return a reference to the file uploaded
    /// </summary>
    /// <param name="createImageReq"></param>
    /// <returns>Image Object uploaded to Azure Storage Blob</returns>
    // POST Images/
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ImageCreateModel createImageReq)
    {
        User user = (User)HttpContext.Items["User"];
        Image image = new()
        {
            Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),
            BlogId = createImageReq.BlogId,
            Created = DateTime.UtcNow.ToShortDateString(),
            Description = createImageReq.Description,
            FileName = createImageReq.File.FileName,
            ContentType = createImageReq.File.ContentType,
            LastModified = DateTime.UtcNow.ToString(),
            Title = createImageReq.Title,
            Type = "Image",
            UserId = user!.Id
        };
        // UPLOAD IMAGE TO AZURE BLOB STORAGE
        // RECEIVE ABSOLUTE URL TO IMAGE UPLOADED TO STORAGE
        // ADD REFERENCE TO STORAGEPATH IN IMAGE OBJECT
        image.StoragePath = await storageService.UploadImage(createImageReq.File, image.Id);
        // UPLOAD IMAGE OBJECT TO COSMOS DB
        var res = await cosmosDb.AddImage(image);
        if (res.StatusCode.ToString().Contains("Created"))
        {
            return CreatedAtAction(nameof(Post), new { image.Id }, image);
        }
        return BadRequest(res.StatusCode);
    }
}
