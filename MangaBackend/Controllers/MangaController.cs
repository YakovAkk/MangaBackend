using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Services.Base;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        private readonly ILogger<MangaController> _logger;

        private readonly IMangaService _mangaService;

        public MangaController(IMangaService mangaService, ILogger<MangaController> logger)
        {
            _mangaService = mangaService;
            _logger = logger;
        }

        [HttpGet("all/favorite")]
        public async Task<IActionResult> GetfavoriteMangas()
        {

            var result = await _mangaService.GetAllFavoriteAsync();

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
        [HttpGet("all/{amount}")]
        public async Task<IActionResult> GetCertainNumber([FromRoute] string amount)
        {
            var amountOfGenres = 0;

            var IsCanParse = Int32.TryParse(amount, out amountOfGenres);

            if (!IsCanParse)
            {
                var message = new
                {
                    result = "Incorrect number of genres"
                };
                return BadRequest(message);
            }

            var result = await _mangaService.GetCertainAmount(amountOfGenres);

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
        public async Task<IActionResult> GetAll()
        {
            var result = await _mangaService.GetAllAsync();

            if (result.Count == 0)
            {
                var message = new
                {
                    result = "The Database doesn't have any manga"
                };
                return BadRequest(message);
            }

            return Ok(result);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetMangaById([FromRoute] string Id)
        {
            var result = await _mangaService.GetByIdAsync(Id);

            if (result.MessageWhatWrong != null && result.MessageWhatWrong.Trim() != "")
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateManga([FromBody] MangaDTO mangaDTO)
        {
            var result = await _mangaService.AddAsync(mangaDTO);

            if (result.MessageWhatWrong != null && result.MessageWhatWrong.Trim() != "")
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMangaById([FromBody] MangaDTO mangaDTO)
        {
            var result = await _mangaService.UpdateAsync(mangaDTO);

            if (result.MessageWhatWrong != null && result.MessageWhatWrong.Trim() != "")
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
        [HttpPut("/api/addGenreToManga")]
        public async Task<IActionResult> AddGenreToManga([FromBody] AddGenreToMangaDTO mangaDTO)
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
                message = "it is working ....",
                data = Environment.GetEnvironmentVariable("ASPNETCORE_DataOfCompipation")
            };

            return Ok(result);
        }

        [HttpDelete("set/favorite/{Id}")]
        public async Task<IActionResult> DeletGenreById([FromRoute] string Id)
        {
            var result = await _mangaService.RemoveFavorite(Id);

            if (!String.IsNullOrEmpty(result.MessageWhatWrong))
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }

        [HttpPost("set/favorite/{Id}")]
        public async Task<IActionResult> AddMangaToFavorite([FromRoute] string Id)
        {
            var result = await _mangaService.AddToFavorite(Id);

            if (!String.IsNullOrEmpty(result.MessageWhatWrong))
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
    }
}
