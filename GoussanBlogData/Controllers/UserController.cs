using AutoMapper;
using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.UserModels;
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
/// Here all interactions with the User Endpoint is handled, like Authentication requests and User Registration and Login
/// </summary>
[Authorize]
[Produces("application/json")]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ICosmosDbService cosmosDb;
    private readonly IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor function needed to initialize services in use by the controller
    /// </summary>
    /// <param name="cosmosDb"></param>
    /// <param name="logger"></param>
    /// <param name="jwtUtils"></param>
    /// <param name="mapper"></param>
    public UserController(ICosmosDbService cosmosDb, ILogger<UserController> logger, IJwtUtils jwtUtils, IMapper mapper)
    {
        this.cosmosDb = cosmosDb;
        _logger = logger;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns a List of currently registered Users
    /// </summary>
    /// <returns></returns>

    // GET /user
    [HttpGet]
    public IActionResult List()
    {
        return Ok(cosmosDb.GetUsersAsync());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    // GET /user/{ID}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await cosmosDb.GetUserAsync(id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns>ID of newly Created User and the User object</returns>
    /// <response code="201">Returns the newly created User</response>
    /// <response code="400">If the Username or Email is already in use, or the API failed at creating the User</response>

    // POST /user/register
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUser user)
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
            Username = user.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Created = DateTime.UtcNow.ToShortDateString(),
            Status = "Pending",
            Confirmationcode = confirmationCode
        };
        var res = await cosmosDb.AddUser(newUser);
        if (res == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Get), new { newUser.Id }, newUser);
    }

    /// <summary>
    /// Authenticates a request and returns a JWT token upon successful authentication
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /authenticate
    ///     {
    ///         "username":"something"
    ///         "password":"something"
    ///      }
    /// </remarks>
    /// <param name="authRequest"></param>
    /// <returns>A JWT Token</returns>
    /// <response code="200">Returns the newly created JWT Token</response>
    /// <response code="401">If the User has not activated his account or wrong Password</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="400">If the application encounters any errors</response>

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

            if (!User.Status.Contains("Activated"))
            {
                return Unauthorized("Pending Account, Please Verify Your Email");
            }


            bool checkPassword = BCrypt.Net.BCrypt.Verify(authRequest.Password, User.Password);
            if (checkPassword)
            {
                var response = _mapper.Map<AuthResponse>(User);
                response.JwtToken = _jwtUtils.GenerateToken(User);
                return Ok(response);
            }
            else
            {
                return Unauthorized();
            }
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Try to Activate the User with Token provided
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /activate
    ///     {
    ///         "token":"123kasdasdh128fh182hf1h"
    ///     }
    /// </remarks>
    /// <param name="token"></param>
    /// <returns>Success Response</returns>
    /// <response code="200">If user is successfully activated</response>
    /// <response code="400">If no token, token is invalid or no user is found</response>

    // POST /user/activate
    [AllowAnonymous]
    [HttpPost("activate")]
    public async Task<IActionResult> UserConfirmation(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("No Token found");
            }
            var response = await cosmosDb.ConfirmUser(token);
            if (string.IsNullOrEmpty(response))
            {
                return BadRequest("User Not Found or invalid Token");
            }
            return Ok(response);
        }
        catch (Exception)
        {
            return BadRequest();
            throw;
        }
    }
}
