using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Services.Services.Base;
using WrapperService.Response;
using WrapperService.StatusCode;
using WrapperService.Wrapper;

namespace MangaBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly ILogger<GenresController> _logger;
    private readonly IGenreService _genreService;
    private readonly ILogsTool _logTool;

    public GenresController(IGenreService genreService, ILogger<GenresController> logger, ILogsTool tool)
    {
        _genreService = genreService;
        _logger = logger;
        _logTool = tool;
    }

    [HttpGet("favorite")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetfavoriteGentes()
    {
        _logTool.NameOfMethod = nameof(GetfavoriteGentes);
        _logTool.WriteToLog(_logger, LogPosition.Begin);

        var result = await _genreService.GetAllFavoriteAsync();

        var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");
        _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {  
            return NotFound(wrapperResult);
        }
        return Ok(wrapperResult);
    }

    [HttpGet("pagination/{pagesize}/{page}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertainNumber([FromRoute] string pagesize, string page)
    {
        _logTool.NameOfMethod = nameof(GetCertainNumber);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"pagesize = {pagesize}, page = {page} ");
        var pageSize = 0;

        var IsCanParsePageSize = Int32.TryParse(pagesize, out pageSize);

        if (!IsCanParsePageSize && pageSize < 0)
        {
            var ErrorMessage = "Incorrect number of pagesize";

            _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)CodeStatus.ErrorWithData} {ErrorMessage}");

            var badResponse = WrapperResponseService.WrapResponseEmpty(null, ErrorMessage);

            return BadRequest(badResponse);
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

            _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)message.StatusCode} {message.ErrorMessage}");
            return BadRequest(message);
        }

        var result = await _genreService.GetCertainPage(pageSize, numberOfPage);

        var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");

        _logTool.WriteToLog(_logger, LogPosition.End, $" Status Code = {(int)wrapperResult.StatusCode}{wrapperResult}");

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
        _logTool.NameOfMethod = nameof(GetAll);

        _logTool.WriteToLog(_logger, LogPosition.Begin);
        var result = await _genreService.GetAllAsync();

        var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");

        _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");

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
        _logTool.NameOfMethod = nameof(GetGenreById);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");
        try
        {
            var result = await _genreService.GetByIdAsync(Id);
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");
            _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("set/favorite/{Id}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddGenreToFavorite([FromRoute] string Id)
    {
        _logTool.NameOfMethod = nameof(AddGenreToFavorite);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");
        try
        {
            var result = await _genreService.AddToFavorite(Id);
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");
            _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, $" Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("filtrarion/{name}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionGenreByName([FromRoute] string name)
    {
        _logTool.NameOfMethod = nameof(FiltrarionGenreByName);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"name = {name}");

        try
        {
            var result = await _genreService.FiltrationByName(name);
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");
            _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, $" Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("set/favorite/{Id}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFavoriteGenreById([FromRoute] string Id)
    {
        _logTool.NameOfMethod = nameof(DeleteFavoriteGenreById);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");
        try
        {
            var result = await _genreService.RemoveFavorite(Id);
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");
            _logTool.WriteToLog(_logger, LogPosition.End, $" Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End,  $" Status Code = {(int)wrapperResult.StatusCode}{wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    //[HttpPut]
    //[ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    //public async Task<IActionResult> UpdateGenreById([FromBody] GenreDTO mangaDTO)
    //{
    //    try
    //    {
    //        var result = await _genreService.UpdateAsync(mangaDTO);
    //        var wrapperResult = _wrapper.WrapTheResponseModel(result);
    //        return Ok(wrapperResult);
    //    }
    //    catch (Exception ex)
    //    {
    //        var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

    //        return NotFound(wrapperResult);
    //    }
    //}
}
