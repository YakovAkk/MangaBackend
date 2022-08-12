using Data.Models;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.Services.Base;

namespace Services.Services
{
    public class MangaService : BaseService<MangaModel, MangaDTO>, IMangaService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMangaRepository _mangaRepository;
        public MangaService(IMangaRepository repository, IGenreRepository genreRepository) : base(repository)
        {
            _genreRepository = genreRepository;
            _mangaRepository = repository;
        }

        public override async Task<MangaModel> AddAsync(MangaDTO item)
        {
            var genres = (await _genreRepository.GetAllAsync()).Where(g => item.genres_id.Contains(g.Id)).ToList();

            if (genres.Count == 0)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The database doesn't contain any genres"
                };
            }

            var model = new MangaModel()
            {
                Id = item.Id,
                Name = item.Name,
                PathToTitlePicture = item.PathToTitlePicture,
                Description = item.Description,
                MessageWhatWrong = "",
                PathToFoldersWithGlava = item.PathToFoldersWithGlava,
                Genres = genres
            };

            return await _repository.CreateAsync(model);
        }
        public async Task<MangaModel> AddGenresToManga(AddGenreToMangaDTO mangaDTO)
        {
            var manga = await _mangaRepository.GetByIdAsync(mangaDTO.MangaId);

            if (manga == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The manga isn't contained in the database!"
                };
            }

            var allGenres = await _genreRepository.GetAllAsync();

            manga.Genres.AddRange(allGenres.Where(g => mangaDTO.Genres_id.Contains(g.Id)));

            var res = await _mangaRepository.UpdateAsync(manga);

            if (!String.IsNullOrEmpty(res.MessageWhatWrong))
            {
                return new MangaModel()
                {
                    MessageWhatWrong = res.MessageWhatWrong
                };
            }

            return res;
        }
        public async override Task<MangaModel> UpdateAsync(MangaDTO item)
        {
            if (String.IsNullOrEmpty(item.Id))
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "Id was null or empty"
                };
            }

            var genres = (await _genreRepository.GetAllAsync()).Where(g => item.genres_id.Contains(g.Id)).ToList();

            if (genres.Count == 0)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The database doesn't contain any genres"
                };
            }

            var manga = await _repository.GetByIdAsync(item.Id);

            if (!String.IsNullOrEmpty(manga.MessageWhatWrong))
            {
                return new MangaModel()
                {
                    MessageWhatWrong = manga.MessageWhatWrong
                };
            };

            manga.Description = item.Description;
            manga.Name = item.Name;
            manga.PathToTitlePicture = item.PathToTitlePicture;
            manga.Genres = genres;
            manga.PathToTitlePicture = item.PathToTitlePicture;

            return await _repository.UpdateAsync(manga);
        }
    }
}
