using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.FillerService.Base;

namespace MangaBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FillerController : ControllerBase
{
    private readonly IFillerService _fillerService;
    private readonly ILogger<FillerController> _logger;
    public FillerController(IFillerService fillerService, ILogger<FillerController> logger)
    {
        _fillerService = fillerService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> FillTheDatabase()
    {
        _logger.LogDebug("FillTheDatabase was begun to fill database");

        var resultGenres = await _fillerService.AddGenres();

        var result = new ResponseFillDTO
        {
            IsSuccess = true,
            MessageWhatWrong = ""
        };

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

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        _logger.LogDebug($"FillTheDatabase was ended to fill database");

        return Ok(result);
    }
}

