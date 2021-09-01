using AutoMapper;
using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.UserModels;
using GoussanBlogData.Services;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GoussanBlogData.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ICosmosDbService cosmosDb;
    private readonly IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;

    public UserController(ICosmosDbService cosmosDb, ILogger<UserController> logger, IJwtUtils jwtUtils, IMapper mapper)
    {
        this.cosmosDb = cosmosDb;
        _logger = logger;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }

    // GET /user
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return Ok(await cosmosDb.GetUsersAsync("SELECT * FROM c"));
    }
    // GET /user/{ID}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await cosmosDb.GetUserAsync(id));
    }

    // POST /user/register
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Createuser user)
    {
        var checkUser = await cosmosDb.CheckUser(user.Username, user.Email);
        if (checkUser == null || checkUser.Any())
        {
            return BadRequest("Username or Email already in Use");
        }
        var Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        var confirmationCode = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        User newUser = new()
        {
            Id = Id,
            Email = user.Email,
            UserName = user.Username,
            PassWord = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Created = DateTime.UtcNow,
            Status = "Pending",
            ConfirmationCode = confirmationCode
        };
        var res = await cosmosDb.AddUser(newUser);
        if (res == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Get), new { newUser.Id }, newUser);
    }

    // POST /user/authenticate
    [AllowAnonymous]
    [HttpPost("authenticate")]

    public async Task<IActionResult> Authenticate([FromBody] AuthRequestModel authRequest)
    {
        try
        {
            IEnumerable<User> userList = await cosmosDb.GetUserByName(authRequest.Username);
            if (userList == null || !userList.Any())
            {
                return NotFound();
            }
            User User = userList.First();

            if (User.Status == null || User.Status != "Active")
            {
                return Unauthorized("Pending Account, Please Verify Your Email");
            }


            bool checkPassword = BCrypt.Net.BCrypt.Verify(authRequest.Password, User.PassWord);
            if (checkPassword)
            {
                var response = _mapper.Map<AuthResponse>(User);
                response.JwtToken = _jwtUtils.GenerateToken(User);

                return Ok(response);
            }
            return BadRequest();
        }
        catch (Exception)
        {
            return BadRequest();
        }
        
    }
}
