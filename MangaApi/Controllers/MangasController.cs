using Microsoft.AspNetCore.Mvc;
using Services.Core;
using Services.Services;
using Services.Services.Base;
using System.Net;
using WrapperService.Model.ResponseModel;
using WrapperService.Wrapper;

namespace MangaBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MangasController : ControllerBase
{
    private readonly IMangaService _mangaService;

    public MangasController(IMangaService mangaService)
    {
        _mangaService = mangaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mangaService.GetAllAsync();

        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {    
            return NotFound(wrapperResult);
        }
        return Ok(wrapperResult);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMangaById([FromRoute] string Id)
    {
        try
        {
            var result = await _mangaService.GetByIdAsync(Id);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("pagination/{pagesize}/{page}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedMangaList([FromRoute] string pagesize, string page)
    {
        try
        {
            var result = await _mangaService.GetPagiantedMangaList(pagesize, page);

            var wrapperResult = WrapperResponseService.Wrap<object>(result);

            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return BadRequest(wrapperResult);
        }
    }

    [HttpGet("filtrarionbyname/{name}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionMangaByName([FromRoute] string name)
    {
        try
        {
            var result = await _mangaService.FiltrationByName(name);
            var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("filtrarionbydate/{year}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionMangaByDate([FromRoute] string year)
    {
        try
        {
            var result = await _mangaService.FiltrationByDate(year);
            var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

            if (wrapperResult.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }
}
