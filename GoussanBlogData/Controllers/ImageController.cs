using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoussanBlogData.Controllers;
[Authorize]
[Route("[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    // GET: image/
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET image/{id}
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST image/
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT image/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE image/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
