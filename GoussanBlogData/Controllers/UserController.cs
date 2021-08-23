using AutoMapper;
using GoussanBlogData.Models.UserModels;
using GoussanBlogData.Services;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GoussanBlogData.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ICosmosDbService cosmosDb;
    private IJwtUtils _jwtUtils;
    private IMapper _mapper;

    public UserController(ICosmosDbService cosmosDb, ILogger<UserController> logger, IJwtUtils jwtUtils, IMapper mapper)
    {
        this.cosmosDb = cosmosDb;
        _logger = logger;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }

    // GET /api/user
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return Ok(await cosmosDb.GetUsersAsync("SELECT * FROM c"));
    }
    // GET /api/user/{ID}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await cosmosDb.GetUserAsync(id));
    }

    // POST /api/user/register
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Createuser user)
    {
        var checkUser = await cosmosDb.CheckUser(user.Username, user.Email);
        if(checkUser == null || checkUser.Any())
        {
            return BadRequest("Username or Email already in Use");
        }
        var Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        User newUser = new()
        {
            Id = Id,
            Email = user.Email,
            UserName = user.Username,
            PassWord = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Created = DateTime.UtcNow.ToShortDateString(),
        };
        var res = await cosmosDb.AddUser(newUser);
        if (res == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Get), new { Id = newUser.Id }, newUser);
    }

    // GET /api/user/authenticate
    [AllowAnonymous]
    [HttpPost("authenticate")]

    public async Task<IActionResult> Authenticate([FromBody] AuthRequestModel authRequest)
    {
        var userList = await cosmosDb.GetUserByName(authRequest.Username);
        if (userList == null || !userList.Any())
        {
            return NotFound();
        }
        var User = userList.FirstOrDefault();
        var checkPassword = BCrypt.Net.BCrypt.Verify(authRequest.Password, User.PassWord);
        if (checkPassword)
        {
            var response = _mapper.Map<AuthResponse>(User);
            response.JwtToken = _jwtUtils.GenerateToken(User);
            return Ok(response);
        }
        return BadRequest();
    }
}
