using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.FillerService.Base;
using Services.Services.Base;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FillerController : ControllerBase
    {
        private readonly IFillerService _fillerService;

        public FillerController(IFillerService fillerService)
        {
            _fillerService = fillerService;
        }

        [HttpPost]
        public async Task<IActionResult> FillTheDatabase()
        {
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
            }

            var resultMangas = await _fillerService.AddMangas();

            if (!resultMangas.IsSuccess)
            {
                result.IsSuccess = false;
                result.MessageWhatWrong += $"      Mangas wasn't added because {resultMangas.MessageWhatWrong}";
            }

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> ClearDatabase()
        {
            var result = await _fillerService.DeleteAll();

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
