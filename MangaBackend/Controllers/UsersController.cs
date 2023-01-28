﻿using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.LogsTools;
using Services.DTO;
using Services.Services.Base;

namespace MangaBackend.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();

        var wrapperResult = _userWrapper.WrapTheResponseListOfModels(result);

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpPost("registration")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Registration([FromBody] UserRegistrationDTO user)
    {
        try
        {
            var result = await _userService.CreateAsync(user);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return BadRequest(wrapperResult);
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO user)
    {
        try
        {
            var result = await _userService.LoginAsync(user);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UserEntity userEntity)
    {
        try
        {
            var result = await _userService.UpdateAsync(userEntity);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("favorite/genre")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddGenreToFavorite([FromBody] FavoriteDTO addTOFavoriteDTO)
    {
        try
        {
            var result = await _userService.AddGenreToFavoriteAsync(addTOFavoriteDTO);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("favorite/manga")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMangaToFavorite([FromBody] FavoriteDTO addTOFavoriteDTO)
    {
        try
        {
            var result = await _userService.AddMangaToFavoriteAsync(addTOFavoriteDTO);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/genre/{userid}/{genreid}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveGenreFromFavorite([FromRoute] string userid, string genreid)
    {
        try
        {
            var result = await _userService.RemoveGenreFromFavoriteAsync(userid, genreid);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/manga/{userid}/{mangaid}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveMangaFromFavorite([FromRoute] string userid, string mangaid)
    {
        try
        {
            var result = await _userService.RemoveMangaFromFavoriteAsync(userid, mangaid);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("favorite/genre/{userid}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteGenre([FromRoute] string userid)
    {
        var result = await _userService.GetAllFavoriteGenreAsync(userid);

        var wrapperResult = _genreWrapper.WrapTheResponseListOfModels(result);

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpGet("favorite/manga/{userid}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteManga([FromRoute] string userid)
    {
        var result = await _userService.GetAllFavoriteMangaAsync(userid);

        var wrapperResult = _mangaWrapper.WrapTheResponseListOfModels(result);

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }
}
