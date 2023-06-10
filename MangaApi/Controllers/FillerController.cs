using Microsoft.AspNetCore.Mvc;
using Services.Model.InputModel;
using Services.Model.ViewModel;
using Services.Services;
using Services.Services.Base;
using WrapperService.Wrapper;

namespace MangaBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HelperController : ControllerBase
{
    private readonly IFillerService _fillerService;
    private readonly ILogger<HelperController> _logger;
    public HelperController(IFillerService fillerService, ILogger<HelperController> logger)
    {
        _fillerService = fillerService;
        _logger = logger;
    }

    [HttpPost("filldata")]
    public async Task<IActionResult> FillTheDatabase()
    {
        _logger.LogDebug("FillTheDatabase was begun to fill database");

        var result = new ResponseViewModel
        {
            IsSuccess = true,
            MessageWhatWrong = ""
        };

        var resultGenres = await _fillerService.AddGenres();

        if (!resultGenres.IsSuccess)
        {
            result.IsSuccess = false;
            result.MessageWhatWrong += $"Genres wasn't added because {resultGenres.MessageWhatWrong}";
            _logger.LogDebug($"Genres wasn't added because {resultGenres.MessageWhatWrong} ");
        }

        var resultMangas = await _fillerService.AddMangas();

        if (!resultMangas.IsSuccess)
        {
            result.IsSuccess = false;
            result.MessageWhatWrong += $" Mangas wasn't added because {resultMangas.MessageWhatWrong}";
            _logger.LogDebug($"Mangas wasn't added because {resultMangas.MessageWhatWrong}");
        }

        //var resultAdmin = await _fillerService.AddAdmin();

        //if (!resultAdmin.IsSuccess)
        //{
        //    result.IsSuccess = false;
        //    result.MessageWhatWrong += $" Admin wasn't added because {resultMangas.MessageWhatWrong}";
        //    _logger.LogDebug($"Admin wasn't added because {resultMangas.MessageWhatWrong}");
        //}

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        _logger.LogDebug($"FillTheDatabase was ended to fill database");

        return Ok(result);
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(UserInputModel user)
    {
        var result = await _fillerService.DeleteUser(user);
        var wrapperResult = WrapperResponseService.Wrap<object>(result);
        return Ok(wrapperResult);
    }
}

