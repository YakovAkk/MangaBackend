using Data.Helping.Extension;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Base;
using WrapperService.Model.InputModel;
using WrapperService.Model.ResponseModel;
using WrapperService.StatusCode;
using WrapperService.Wrapper;

namespace MangaBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet("pagination/{pagesize}/{page}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertainNumber([FromRoute] string pagesize, string page)
    {
        var result = await _genreService.GetCertainPage(pagesize, page);

        var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
        {
            Data = result,
        });

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _genreService.GetAllAsync();

        var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel() 
        {
           Data = result,
        });


        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGenreById([FromRoute] string Id)
    {
        
        try
        {
            var result = await _genreService.GetByIdAsync(Id);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
            {
                Data = result.ToList(),
            });
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            return NotFound(wrapperResult);
        }
    }

    //[HttpPost]
    //[ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    //public async Task<IActionResult> CreateGenre([FromBody] GenreDTO mangaDTO)
    //{
    //    try
    //    {
    //        var result = await _genreService.AddAsync(mangaDTO);
    //        var wrapperResult = _wrapper.WrapTheResponseModel(result);
    //        return Ok(wrapperResult);
    //    }
    //    catch (Exception ex)
    //    {
    //        var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);

    //        return NotFound(wrapperResult);
    //    }
    //}

    {
        _logTool.NameOfMethod = nameof(AddGenreToFavorite);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");
        try
        {
            var result = await _genreService.AddToFavorite(Id);
            var wrapperResult = _wrapper.WrapTheResponseModel(result);
            _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, $" Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("filtrarion/{name}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionGenreByName([FromRoute] string name)
    {
            var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, $" Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");

    [HttpDelete("set/favorite/{Id}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFavoriteGenreById([FromRoute] string Id)
    {
        _logTool.NameOfMethod = nameof(DeleteFavoriteGenreById);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");
        try
        {
            var result = await _genreService.RemoveFavorite(Id);
            var wrapperResult = _wrapper.WrapTheResponseModel(result);
            _logTool.WriteToLog(_logger, LogPosition.End, $" Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End,  $" Status Code = {(int)wrapperResult.StatusCode}{wrapperResult}");
            return NotFound(wrapperResult);
        }
    }
}
