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

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var result = await _genreService.GetAllAsync();

            var wrapResult = _wrapper.WrapTheResponseListOfModels(result);

            if(wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> getGenreById([FromRoute] string Id)
        {
            var result = await _genreService.GetByIdAsync(Id);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }

        [HttpPost]
        public async Task<IActionResult> createManga([FromBody] GenreDTO mangaDTO)
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
            var result = await _genreService.DeleteAsync(Id);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }

        [HttpPut]
        public async Task<IActionResult> updateMangaById([FromBody] GenreDTO mangaDTO)
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
