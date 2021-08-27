using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoussanBlogData.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    // GET: api/<ImageController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<ImageController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ImageController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<ImageController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ImageController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
