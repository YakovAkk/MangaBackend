using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Services.Base;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly ILogger<GenreController> _logger;

        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService, ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        [HttpGet("all/{amount}")]
        public async Task<IActionResult> getAll([FromRoute] string amount)
        {
            int amountOfGenres = 0;

            var IsCanParse = Int32.TryParse(amount, out amountOfGenres);

            if (!IsCanParse)
            {
                var message = new
                {
                    result = "Incorrect anount of genres"
                };
                return BadRequest(message);
            }

            var result = await _genreService.GetCertainAmount(amountOfGenres);

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

        [HttpGet("all")]
        public async Task<IActionResult> getAll()
        {
            var result = await _genreService.GetAllAsync();

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
            var result = await _genreService.GetByIdAsync(Id);

            if (!String.IsNullOrEmpty(result.MessageWhatWrong))
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> createManga([FromBody] GenreDTO mangaDTO)
        {
            var result = await _genreService.AddAsync(mangaDTO);

            if (!String.IsNullOrEmpty(result.MessageWhatWrong))
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> deleteMangaById([FromRoute] string Id)
        {
            var result = await _genreService.DeleteAsync(Id);

            if (!String.IsNullOrEmpty(result.MessageWhatWrong))
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> updateMangaById([FromBody] GenreDTO mangaDTO)
        {
            var result = await _genreService.UpdateAsync(mangaDTO);

            if (!String.IsNullOrEmpty(result.MessageWhatWrong))
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
    }
}
