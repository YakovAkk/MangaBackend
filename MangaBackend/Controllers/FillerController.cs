using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Services.Base;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FillerController : ControllerBase
    {
        private readonly IMangaService _mangaService;
        private readonly IGenreService _genreService;

        public FillerController(IMangaService mangaService, IGenreService genreService)
        {
            _mangaService = mangaService;
            _genreService = genreService;
        }

        [HttpPost]
        public async Task<IActionResult> createManga()
        {
            var ganreDTO = new GenreDTO()
            {
                Name = "Action"
            };
            var resultGenre = await _genreService.AddAsync(ganreDTO);
            if (!String.IsNullOrEmpty(resultGenre.MessageWhatWrong))
            {
                return BadRequest(resultGenre.MessageWhatWrong);
            }

            var genres = await _genreService.GetAllAsync();

            if (genres.Count == 0)
            {
                var message = new
                {
                    genres = "The Database doesn't have any genres"
                };
                return BadRequest(message);
            }

            var genres_id = new List<string>();
            foreach (var genre in genres)
            {
                genres_id.Add(genre.Id);
            }

            var itemToFoldersWithGlava = new GlavaMangaModel()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "Manga/Attack+of+the+Titans/Glava+1/01.jpg",
                AmountOfPictures = 54
            };

            var PathToFoldersWithGlava = new List<GlavaMangaModel>();
            PathToFoldersWithGlava.Add(itemToFoldersWithGlava);

            var mangaDTO = new MangaDTO()
            {
                Name = "Attack of the Titans",
                PathToTitlePicture = "Manga/Attack+of+the+Titans/TitleImage.jpg",
                genres_id = genres_id,
                PathToFoldersWithGlava = PathToFoldersWithGlava,
                Description = "Давным-давно человечество было всего лишь «их» кормом, до тех пор, пока оно не построило гигантскую стену вокруг своей страны. С тех пор прошло сто лет мира и большинство людей жили счастливой, беззаботной жизнью. Но за долгие годы спокойствия пришлось заплатить огромную цену, и в 845 году они снова познали чувство ужаса и беспомощности – стена, которая была их единственным спасением, пала. «Они» прорвались. Половина человечества съедена, треть территории навсегда потеряна..."
            };

            var resultManga = await _mangaService.AddAsync(mangaDTO);

            if (!String.IsNullOrEmpty(resultManga.MessageWhatWrong))
            {
                return BadRequest(resultManga.MessageWhatWrong);
            }

            var res = new
            {
                mess = "All Items was added to Database"
            };

            return Ok(res);
        }
    }
}
