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
        private readonly IFillerSwervice _fillerService;

        public FillerController(IFillerSwervice fillerService)
        {
            _fillerService = fillerService;
        }

        [HttpPost]
        public async Task<IActionResult> createManga()
        {
            var resultGenres = await _fillerService.AddGenres();

            if (!resultGenres.IsSuccess)
            {
                var res = new
                {
                    mess = $"Genres wasn't added because {resultGenres.MessageWhatWrong}"
                };
                return BadRequest(res);
            }

            var resultMangas = await _fillerService.AddMangas();

            if (!resultMangas.IsSuccess)
            {
                var res = new
                {
                    mess = $"Mangas wasn't added because {resultMangas.MessageWhatWrong}"
                };
                return BadRequest(res);
            }

            var result = new
            {
                mess = $"Everything was added"
            };

            return Ok(result);
        }
    }
}
