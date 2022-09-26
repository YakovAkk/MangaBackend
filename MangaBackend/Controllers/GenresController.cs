using Data.Models;
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
    public class GenresController : ControllerBase
    {
        private readonly IWrapperGenreService _wrapper;
        private readonly ILogger<GenresController> _logger;

        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService, ILogger<GenresController> logger, IWrapperGenreService wrapper)
        {
            _genreService = genreService;
            _logger = logger;
            _wrapper = wrapper;
        }

        [HttpGet("favorite")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetfavoriteGentes()
        {
            var result = await _genreService.GetAllFavoriteAsync();

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return NotFound(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpGet("pagination/{pagesize}/{page}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCertainNumber([FromRoute] string pagesize , string page)
        {
            var pageSize = 0;

            var IsCanParsePageSize = Int32.TryParse(pagesize, out pageSize);

            if (!IsCanParsePageSize && pageSize < 0)
            {
                var message = new ResponseModel()
                {
                    data = null,
                    ErrorMessage = "Incorrect number of pagesize",
                    StatusCode = CodeStatus.ErrorWithData      
                };
                return BadRequest(message);
            }

            var numberOfPage = 0;

            var IsCanParseNumberOfPage = Int32.TryParse(page, out numberOfPage);

            if (!IsCanParseNumberOfPage && numberOfPage < 0)
            {
                var message = new ResponseModel()
                {
                    data = null,
                    ErrorMessage = "Incorrect number of page",
                    StatusCode = CodeStatus.ErrorWithData
                };
                return BadRequest(message);
            }

            var result = await _genreService.GetCertainPage(pageSize, numberOfPage);

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return NotFound(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _genreService.GetAllAsync();

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return NotFound(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGenreById([FromRoute] string Id)
        {
            try
            {
                var result = await _genreService.GetByIdAsync(Id);
                var wrapperResult = _wrapper.WrapTheResponseModel(result);
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

                return NotFound(wrapperResult);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateGenre([FromBody] GenreDTO mangaDTO)
        {
            try
            {
                var result = await _genreService.AddAsync(mangaDTO);
                var wrapperResult = _wrapper.WrapTheResponseModel(result);
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

                return NotFound(wrapperResult);
            }
        }

        [HttpPost("set/favorite/{Id}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddGenreToFavorite([FromRoute] string Id)
        {
            try
            {
                var result = await _genreService.AddToFavorite(Id);
                var wrapperResult = _wrapper.WrapTheResponseModel(result);
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

                return NotFound(wrapperResult);
            }
        }

        [HttpPost("filtrarion/{name}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> FiltrarionGenreByName([FromRoute] string name)
        {
            var result = await _genreService.FiltrationByName(name);

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpDelete("set/favorite/{Id}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletGenreById([FromRoute] string Id)
        {
            try
            {
                var result = await _genreService.RemoveFavorite(Id);
                var wrapperResult = _wrapper.WrapTheResponseModel(result);
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

                return NotFound(wrapperResult);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateGenreById([FromBody] GenreDTO mangaDTO)
        {
            try
            {
                var result = await _genreService.UpdateAsync(mangaDTO);
                var wrapperResult = _wrapper.WrapTheResponseModel(result);
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

                return NotFound(wrapperResult);
            }
        }
    }
}
