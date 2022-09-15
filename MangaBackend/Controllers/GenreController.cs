using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Response;
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

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpGet("all/{amount}")]
        public async Task<IActionResult> GetCertainNumber([FromRoute] string amount)
        {
            var amountOfGenres = 0;

            var IsCanParse = Int32.TryParse(amount, out amountOfGenres);

            if (!IsCanParse)
            {
                var message = new ResponseModel()
                {
                    data = null,
                    ErrorMessage = "Incorrect number of genres",
                    StatusCode = CodeStatus.ErrorWithData      
                };
                return BadRequest(message);
            }

            var result = await _genreService.GetCertainAmount(amountOfGenres);

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult.ErrorMessage);
            }

            return Ok(wrapperResult);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _genreService.GetAllAsync();

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetGenreById([FromRoute] string Id)
        {
            var result = await _genreService.GetByIdAsync(Id);

            var wrapperResult = _wrapper.WrapTheResponseModel(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
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

        [HttpPost("set/favorite/{Id}")]
        public async Task<IActionResult> AddGenreToFavorite([FromRoute] string Id)
        {
            var result = await _genreService.AddToFavorite(Id);

            var wrapperResult = _wrapper.WrapTheResponseModel(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpDelete("set/favorite/{Id}")]
        public async Task<IActionResult> DeletGenreById([FromRoute] string Id)
        {
            var result = await _genreService.RemoveFavorite(Id);

            var wrapperResult = _wrapper.WrapTheResponseModel(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
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
