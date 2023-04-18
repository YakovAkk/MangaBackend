using Microsoft.AspNetCore.Mvc;
using Services.Services.Base;
using WrapperService.Model.ResponseModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JavaTestController : ControllerBase
    {
        private readonly IJavaTestService _service;
        public JavaTestController(IJavaTestService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostTests(List<string> testCases)
        {
            var data = await _service.PostTests(testCases);

            return Ok(data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTests()
        {
            var data = await _service.GetTests();

            return Ok(data);
        }

        [HttpGet("LastTest")]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLastTest()
        {
            var data = await _service.GetLastTest();

            return Ok(data);
        }
    }
}
