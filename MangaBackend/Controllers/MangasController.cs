using Data.Helping.Extension;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Base;
using System.Net;
using WrapperService.Model.InputModel;
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
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mangaService.GetAllAsync();

        var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
        {
            Data = result
        });

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {    
            return NotFound(wrapperResult);
        }
        return Ok(wrapperResult);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMangaById([FromRoute] string Id)
    {
        try
        {
            var result = await _mangaService.GetByIdAsync(Id);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel() 
            {
                Data = result.ToList()
            });
            
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("pagination/{pagesize}/{page}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertainNumber([FromRoute] string pagesize, string page)
    {
        var result = await _mangaService.GetCertainPage(pagesize, page);

        var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
        {
            Data = result
        });

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpGet("filtrarionbyname/{name}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionMangaByName([FromRoute] string name)
    {
        try
        {
            var result = await _mangaService.FiltrationByName(name);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
            {
                Data = result
            });
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("filtrarionbydate/{year}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionMangaByDate([FromRoute] string year)
    {
        try
        {
            var result = await _mangaService.FiltrationByDate(year);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel() 
            { 
                Data = result
            });

            if (wrapperResult.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            return NotFound(wrapperResult);
        }
    }
}
