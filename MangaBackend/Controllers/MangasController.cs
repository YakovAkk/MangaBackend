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

        if (wrapperResult.StatusCode != CodeStatus.Successful)
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

    [HttpPost("filtrarionbyname/{name}")]
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

    [HttpPost("filtrarionbydate/{year}")]
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

            if (wrapperResult.StatusCode != CodeStatus.Successful)
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

}
