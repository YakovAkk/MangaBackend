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
    public class MangasController : ControllerBase
    {
        private readonly IWrapperMangaService _wrapper;
        private readonly ILogger<MangasController> _logger;

        private readonly IMangaService _mangaService;

        public MangasController(IMangaService mangaService, ILogger<MangasController> logger, IWrapperMangaService wrapper)
        {
            _mangaService = mangaService;
            _logger = logger;
            _wrapper = wrapper;
        }

        [HttpGet("favorite")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetfavoriteMangas()
        {

            var result = await _mangaService.GetAllFavoriteAsync();

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
            var result = await _mangaService.GetAllAsync();

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return NotFound(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> getMangaById([FromRoute] string Id)
        {
            var result = await _mangaService.GetByIdAsync(Id);

            var wrapResult = _wrapper.WrapTheResponseModel(result);

            if (wrapResult.StatusCode != CodeStatus.Successful)
            {
                return NotFound(wrapResult);
            }

            return Ok(wrapResult);
        }

        [HttpGet("pagination/{pagesize}/{page}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCertainNumber([FromRoute] string pagesize, string page)
        {
            var pageSize = 0;

            var IsCanParsePageSize = Int32.TryParse(pagesize, out pageSize);

            if (!IsCanParsePageSize && pageSize < 0)
            {
                var message = new ResponseModel()
                {
                    Data = null,
                    Message = "Incorrect number of pagesize",
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
                    Data = null,
                    Message = "Incorrect number of page",
                    StatusCode = CodeStatus.ErrorWithData
                };
                return BadRequest(message);
            }

            var result = await _mangaService.GetCertainPage(pageSize, numberOfPage);

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return NotFound(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
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

        [HttpPost("set/favorite/{Id}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
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

        [HttpPost("filtrarionbyname/{name}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> FiltrarionMangaByName([FromRoute] string name)
        {
            var result = await _mangaService.FiltrationByName(name);

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }


        [HttpPost("filtrarionbydate/{year}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> FiltrarionMangaByDate ([FromRoute] string year)
        {
            var yearnum = 0;

            var IsCanParsePageSize = Int32.TryParse(year, out yearnum);

            if (!IsCanParsePageSize && yearnum < 0)
            {
                var message = new ResponseModel()
                {
                    Data = null,
                    Message = "Incorrect year",
                    StatusCode = CodeStatus.ErrorWithData
                };
                return BadRequest(message);
            }

            var result = await _mangaService.FiltrationByDate(yearnum);

            var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
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

        [HttpPut("addGenreToManga")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
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
    }
}
