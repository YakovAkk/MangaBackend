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
public class MangasController : ControllerBase
{
    private readonly ILogger<MangasController> _logger;
    private readonly ILogsTool _logTool;
    private readonly IMangaService _mangaService;

    public MangasController(IMangaService mangaService, ILogger<MangasController> logger, ILogsTool tool)
    {
        _mangaService = mangaService;
        _logger = logger;
        _logTool = tool;
    }

    [HttpGet("favorite")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetfavoriteMangas()
    {
        _logTool.NameOfMethod = nameof(GetfavoriteMangas);

        _logTool.WriteToLog(_logger, LogPosition.Begin);

        var result = await _mangaService.GetAllFavoriteAsync();

        var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");

        _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");

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

        var result = await _mangaService.GetAllAsync();

        var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");

        _logTool.WriteToLog(_logger, LogPosition.End,  $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {    
            return NotFound(wrapperResult);
        }
        return Ok(wrapperResult);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMangaById([FromRoute] string Id)
    {
        _logTool.NameOfMethod = nameof(GetMangaById);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");

        try
        {
            var result = await _mangaService.GetByIdAsync(Id);
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");
            _logTool.WriteToLog(_logger, LogPosition.End, 
                 $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, 
                 $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("pagination/{pagesize}/{page}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertainNumber([FromRoute] string pagesize, string page)
    {
        _logTool.NameOfMethod = nameof(GetCertainNumber);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"pagesize = {pagesize}, page = {page} ");
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
            _logTool.WriteToLog(_logger, LogPosition.End, 
                 $"Status Code = {(int)message.StatusCode} {message}");

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
            _logTool.WriteToLog(_logger, LogPosition.End, 
                 $"Status Code = {(int)message.StatusCode} {message}");
            return BadRequest(message);
        }

        var result = await _mangaService.GetCertainPage(pageSize, numberOfPage);

        var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");

        _logTool.WriteToLog(_logger, LogPosition.End, 
             $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpPost("set/favorite/{Id}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMangaToFavorite([FromRoute] string Id)
    {
        _logTool.NameOfMethod = nameof(AddMangaToFavorite);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");
        try
        {
            var result = await _mangaService.AddToFavorite(Id);
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");
            _logTool.WriteToLog(_logger, LogPosition.End, 
                 $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, 
                 $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("filtrarionbyname/{name}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionMangaByName([FromRoute] string name)
    {
        _logTool.NameOfMethod = nameof(FiltrarionMangaByName);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Name = {name}");
        try
        {
            var result = await _mangaService.FiltrationByName(name);
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");
            _logTool.WriteToLog(_logger, LogPosition.End,
                 $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, 
                $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("filtrarionbydate/{year}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionMangaByDate([FromRoute] string year)
    {
        _logTool.NameOfMethod = nameof(FiltrarionMangaByDate);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Year = {year}");
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
            _logTool.WriteToLog(_logger, LogPosition.End,
                $"Status Code = {(int)message.StatusCode} {message}");
            return BadRequest(message);
        }

        try
        {
            var result = await _mangaService.FiltrationByDate(yearnum);
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
    public async Task<IActionResult> DeletGenreById([FromRoute] string Id)
    {
        _logTool.NameOfMethod = nameof(DeletGenreById);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");
        try
        {
            var result = await _mangaService.RemoveFavorite(Id);
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(result, "No data");
            _logTool.WriteToLog(_logger, LogPosition.End, 
               $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.WrapResponseEmpty(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, 
                 $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }
}
