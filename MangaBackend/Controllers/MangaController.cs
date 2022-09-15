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
    public class MangaController : ControllerBase
    {
        private readonly IWrapperMangaService _wrapper;
        private readonly ILogger<MangaController> _logger;

        private readonly IMangaService _mangaService;

        public MangaController(IMangaService mangaService, ILogger<MangaController> logger, IWrapperMangaService wrapper)
        {
            _mangaService = mangaService;
            _logger = logger;
            _wrapper = wrapper;
        }

        [HttpGet("all/favorite")]
        public async Task<IActionResult> GetfavoriteMangas()
        {

            var result = await _mangaService.GetAllFavoriteAsync();

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

            var result = await _mangaService.GetCertainAmount(amountOfGenres);

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
            var result = await _mangaService.GetAllAsync();

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> getMangaById([FromRoute] string Id)
        {
            var result = await _mangaService.GetByIdAsync(Id);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }

        [HttpPost]
        public async Task<IActionResult> createManga([FromBody] MangaDTO mangaDTO)
        {
            var result = await _mangaService.AddAsync(mangaDTO);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMangaById([FromBody] MangaDTO mangaDTO)
        {
            var result = await _mangaService.UpdateAsync(mangaDTO);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }

        [HttpPut("/api/addGenreToManga")]
        public async Task<IActionResult> AddGenreToManga([FromBody] AddGenreToMangaDTO mangaDTO)
        {
            var result = await _mangaService.AddGenresToManga(mangaDTO);

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
            var result = await _mangaService.RemoveFavorite(Id);

            var wrapperResult = _wrapper.WrapTheResponseModel(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpPost("set/favorite/{Id}")]
        public async Task<IActionResult> AddMangaToFavorite([FromRoute] string Id)
        {
            var result = await _mangaService.AddToFavorite(Id);

            var wrapperResult = _wrapper.WrapTheResponseModel(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }
    }
}
