
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GoussanBlogData.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class MeController : ControllerBase
{

    // GET /me
    [HttpGet]
    public IActionResult Get()
    {
        var responseHeaders = HttpContext.Request.Headers;
        return Ok(responseHeaders);
    }
}
