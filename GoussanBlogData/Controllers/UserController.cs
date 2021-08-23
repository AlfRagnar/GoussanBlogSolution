using GoussanBlogData.Models.UserModels;
using GoussanBlogData.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GoussanBlogData.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ICosmosDbService cosmosDb;

    public UserController(ICosmosDbService cosmosDb, ILogger<UserController> logger)
    {
        this.cosmosDb = cosmosDb;
        _logger = logger;
    }

    // GET /api/user
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return Ok(await cosmosDb.GetUsersAsync("SELECT c.username FROM User c"));
    }
    // GET /api/user/{ID}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await cosmosDb.GetUserAsync(id));
    }

    // POST /api/user/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        var checkUser = await cosmosDb.CheckUser(user.UserName, user.Email);
        if(checkUser == null || checkUser.Any())
        {
            return BadRequest("Username or Email already in Use");
        }
        var Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        User newUser = new()
        {
            Id = Id,
            Email = user.Email,
            UserName = user.UserName,
            PassWord = BCrypt.Net.BCrypt.HashPassword(user.PassWord),
            Created = DateTime.UtcNow.ToShortDateString(),
            Medias = user.Medias,
        };
        var res = await cosmosDb.AddUser(newUser);
        if (res == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Get), new { Id = newUser.Id }, newUser);
    }

    // GET /api/user/authenticate
    [HttpPost("authenticate")]

    public async Task<IActionResult> Authenticate([FromBody] AuthRequestModel authRequest)
    {
        var userList = await cosmosDb.GetUserByName(authRequest.Username);
        if (userList == null || !userList.Any())
        {
            return NotFound();
        }
        var firstUser = userList.FirstOrDefault();
        var checkPassword = BCrypt.Net.BCrypt.Verify(authRequest.Password, firstUser.PassWord);
        if (checkPassword)
        {
            return Ok(firstUser);
        }
        return BadRequest();
    }
}
