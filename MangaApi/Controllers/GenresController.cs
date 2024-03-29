﻿using Microsoft.AspNetCore.Mvc;
using Services.Services.Base;
using System.Net;
using WrapperService.Model.ResponseModel;
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
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GEtPaginatedGenreList([FromRoute] int pagesize, int page)
    {
        var result = await _genreService.GetPaginatedGenreListAsync(pagesize, page);

        var wrapperResult = WrapperResponseService.Wrap<object>(result);

        return Ok(wrapperResult);
    }

    [HttpGet]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _genreService.GetAllAsync();

        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGenreById([FromRoute] int Id)
    {
        var result = await _genreService.GetByIdAsync(Id);
        var wrapperResult = WrapperResponseService.Wrap<object>(result);
        return Ok(wrapperResult);
    }

    [HttpGet("filtrarion/{name}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionGenreByName([FromRoute] string name)
    {
        var result = await _genreService.FiltrationByNameAsync(name);
        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);
        return Ok(wrapperResult);
    }
}
