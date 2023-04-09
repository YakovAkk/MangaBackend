using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MangaBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{

    private readonly string _path;
    public TestController()
    {
        _path = "./Version.json";
    }

    [HttpGet("/api/test")]
    public async Task<IActionResult> test()
    {
        try
        {
            string text = System.IO.File.ReadAllText(_path);

            var json = JObject.Parse(text);
            var result = new
            {
                message = "it is working ....",
                version = json["version"],
                data = json["data"]
            };

            return Ok(result);
        }
        catch (Exception)
        {

            var result = new
            {
                message = "it is working ...."
            };

            return Ok(result);
        }
    }
}
