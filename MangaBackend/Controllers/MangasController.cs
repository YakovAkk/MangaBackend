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
            try
            {
                var result = await _mangaService.GetByIdAsync(Id);
                var wrapperResult = _wrapper.WrapTheResponseModel(result);
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

                return NotFound(wrapperResult);
            }
        }

        [HttpGet("pagination/{pagesize}/{page}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCertainNumber([FromRoute] string pagesize, string page)
        {
            var pageSize = 0;

            var IsCanParsePageSize = Int32.TryParse(pagesize, out pageSize);

            if (!IsCanParsePageSize || pageSize < 0)
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

            if (!IsCanParseNumberOfPage || numberOfPage < 0)
            {
                var message = new ResponseModel()
                {
                    data = null,
                    ErrorMessage = "Incorrect number of page",
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

        //[HttpPost]
        //[ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        //public async Task<IActionResult> createManga([FromBody] MangaDTO mangaDTO)
        //{
        //    try
        //    {
        //        var result = await _mangaService.AddAsync(mangaDTO);
        //        var wrapperResult = _wrapper.WrapTheResponseModel(result);
        //        return Ok(wrapperResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

        //        return NotFound(wrapperResult);
        //    }
        //}

        [HttpPost("set/favorite/{Id}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddMangaToFavorite([FromRoute] string Id)
        { 
            try
            {
                var result = await _mangaService.AddToFavorite(Id);
                var wrapperResult = _wrapper.WrapTheResponseModel(result);
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

                return NotFound(wrapperResult);
            }
        }

        [HttpPost("filtrarionbyname/{name}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> FiltrarionMangaByName([FromRoute] string name)
        {
            try
            {
                var result = await _mangaService.FiltrationByName(name);

                var wrapperResult = _wrapper.WrapTheResponseListOfModels(result);
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

                return NotFound(wrapperResult);
            }
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
                    data = null,
                    ErrorMessage = "Incorrect year",
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

        //[HttpPut]
        //[ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        //public async Task<IActionResult> UpdateMangaById([FromBody] MangaDTO mangaDTO)
        //{
        //    try
        //    {
        //        var result = await _mangaService.UpdateAsync(mangaDTO);
        //        var wrapperResult = _wrapper.WrapTheResponseModel(result);
        //        return Ok(wrapperResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

        //        return NotFound(wrapperResult);
        //    }
        //}

        //[HttpPut("addGenreToManga")]
        //[ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        //public async Task<IActionResult> AddGenreToManga([FromBody] AddGenreToMangaDTO mangaDTO)
        //{
        //    try
        //    {
        //        var result = await _mangaService.AddGenresToManga(mangaDTO);
        //        var wrapperResult = _wrapper.WrapTheResponseModel(result);
        //        return Ok(wrapperResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

        //        return NotFound(wrapperResult);
        //    }
        //}

        [HttpDelete("set/favorite/{Id}")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletGenreById([FromRoute] string Id)
        {
            try
            {
                var result = await _mangaService.RemoveFavorite(Id);
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
