using Microsoft.AspNetCore.Mvc;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("/api/test")]
        public async Task<IActionResult> test()
        {
            var result = new
            {
                message = "it is working ....",
                data = Environment.GetEnvironmentVariable("ASPNETCORE_DataOfCompipation")
            };

            return Ok(result);
        }
    }
}
