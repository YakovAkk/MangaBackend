using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Services.Base;
using Services.StatusCode;
using Services.Wrappers.Base;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IWrapperGenreService _wrapper;
        private readonly ILogger<GenreController> _logger;

        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService, ILogger<GenreController> logger, IWrapperGenreService wrapper)
        {
            _genreService = genreService;
            _logger = logger;
            _wrapper = wrapper;
        }

        [HttpGet("all/favorite")]
        public async Task<IActionResult> GetfavoriteGentes()
        {
            var result = await _genreService.GetAllFavoriteAsync();

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
        public async Task<IActionResult> GetAll()
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
        public async Task<IActionResult> GetGenreById([FromRoute] string Id)
        {
            var result = await _genreService.GetByIdAsync(Id);

            if (!String.IsNullOrEmpty(result.MessageWhatWrong))
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreDTO mangaDTO)
        {
            var result = await _genreService.AddAsync(mangaDTO);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> deleteMangaById([FromRoute] string Id)
        {
            var result = await _genreService.AddToFavorite(Id);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }
        [HttpDelete("set/favorite/{Id}")]
        public async Task<IActionResult> DeletGenreById([FromRoute] string Id)
        {
            var result = await _genreService.RemoveFavorite(Id);

            if (!String.IsNullOrEmpty(result.MessageWhatWrong))
            {
                return BadRequest(result.MessageWhatWrong);
            }

            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateGenreById([FromBody] GenreDTO mangaDTO)
        {
            var result = await _genreService.UpdateAsync(mangaDTO);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }
    }
}
