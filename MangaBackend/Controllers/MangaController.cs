using Microsoft.AspNetCore.Mvc;
using Services.DTO;
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

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var result = await _mangaService.GetAllAsync();

            var wrapResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
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
        [HttpDelete("{Id}")]
        public async Task<IActionResult> deleteMangaById([FromRoute] string Id)
        {
            var result = await _mangaService.DeleteAsync(Id);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }
        [HttpPut]
        public async Task<IActionResult> updateMangaById([FromBody] MangaDTO mangaDTO)
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
        public async Task<IActionResult> addGenreToManga([FromBody] AddGenreToMangaDTO mangaDTO)
        {
            var result = await _mangaService.AddGenresToManga(mangaDTO);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapResult);
            }

            return Ok(wrapResult);
        }
   
    }
}
