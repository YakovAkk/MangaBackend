using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Services.Base;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        private readonly IMangaService _mangaService;

        public MangaController(IMangaService mangaService)
        {
            _mangaService = mangaService;
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var result = await _mangaService.GetAllAsync();

            if (result.Count == 0)
            {
                var message = new
                {
                    result = "The Database doesn't have any category"
                };
                return BadRequest(message);
            }

            return Ok(result);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> getMangaById([FromRoute] string Id)
        {
            var result = await _mangaService.GetByIdAsync(Id);

            if (result.MessageWhatWrong != null && result.MessageWhatWrong.Trim() != "")
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> createManga([FromBody] MangaDTO mangaDTO)
        {
            var result = await _mangaService.AddAsync(mangaDTO);

            if (result.MessageWhatWrong != null && result.MessageWhatWrong.Trim() != "")
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> deleteMangaById([FromRoute] string Id)
        {
            var result = await _mangaService.DeleteAsync(Id);

            if (result.MessageWhatWrong != null && result.MessageWhatWrong.Trim() != "")
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> updateMangaById([FromBody] MangaDTO mangaDTO)
        {
            var result = await _mangaService.UpdateAsync(mangaDTO);

            if (result.MessageWhatWrong != null && result.MessageWhatWrong.Trim() != "")
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }

        [HttpPut("/api/addGenreToManga")]
        public async Task<IActionResult> addGenreToManga([FromBody] AddGenreToMangaDTO mangaDTO)
        {
            var result = await _mangaService.AddGenresToManga(mangaDTO);

            if (result.MessageWhatWrong != null && result.MessageWhatWrong.Trim() != "")
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }

        [HttpGet("/api/test")]
        public async Task<IActionResult> test() 
        {
            var result = new
            {
                message = "it is working ...."
            };

            return Ok(result);
        }
    }
}
