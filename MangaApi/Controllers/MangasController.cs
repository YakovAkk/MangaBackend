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
    public async Task<IActionResult> GetMangaById([FromRoute] int Id)
    {
        var result = await _mangaService.GetByIdAsync(Id);
        var wrapperResult = WrapperResponseService.Wrap<object>(result);

        return Ok(wrapperResult);
    }

    [HttpGet("pagination/{pagesize}/{page}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedMangaList([FromRoute] int pagesize, int page)
    {
        var result = await _mangaService.GetPagiantedMangaListAsync(pagesize, page);

        var wrapperResult = WrapperResponseService.Wrap<object>(result);

        return Ok(wrapperResult);
    }

    [HttpGet("filtrarionbyname/{name}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionMangaByName([FromRoute] string name)
    {
        var result = await _mangaService.FiltrationByNameAsync(name);
        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);
        return Ok(wrapperResult);
    }

    [HttpGet("filtrarionbydate/{year}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionMangaByDate([FromRoute] int year)
    {
        var result = await _mangaService.FiltrationByDateAsync(year);
        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {
            return BadRequest(wrapperResult);
        }

        return Ok(wrapperResult);
    }
}
