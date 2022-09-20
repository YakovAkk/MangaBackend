using Data.Models;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.Services.Base;
using Services.Storage.Base;

namespace Services.Services
{
    public class MangaService : BaseService<MangaModel, MangaDTO>, IMangaService
    {
        private readonly ILocalStorage _localStorage;
        private readonly IGenreRepository _genreRepository;
        private readonly IMangaRepository _mangaRepository;
        public MangaService(
            IMangaRepository repository,
            IGenreRepository genreRepository, 
            ILocalStorage localStorage) : base(repository)
        {
            _genreRepository = genreRepository;
            _mangaRepository = repository;
            _localStorage = localStorage;
        }

        private MangaModel ConverterModelDTOToModel(MangaDTO item, List<GenreModel> genres)
        {
            var model = new MangaModel()
            {
                Id = item.Id,
                Name = item.Name,
                PathToTitlePicture = item.PathToTitlePicture,
                Description = item.Description,
                MessageWhatWrong = "",
                PathToFoldersWithGlava = item.PathToFoldersWithGlava,
                AgeRating = item.AgeRating,
                Author = item.Author,
                IsFavorite = item.IsFavorite,
                NumbetOfChapters = item.NumbetOfChapters,
                ReleaseYear = item.ReleaseYear,
                Genres = genres
            };

            return model;
        }
        public override async Task<MangaModel> AddAsync(MangaDTO item)
        {
            var genres = (await _genreRepository.GetAllAsync()).Where(g => item.genres_id.Contains(g.Id)).ToList();

            if (!genres.Any())
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The database doesn't contain the genres"
                };
            }

            var model = ConverterModelDTOToModel(item, genres);

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
        public async override Task<IList<MangaModel>> AddRange(IList<MangaDTO> list)
        {
            var listModels = new List<MangaModel>();

            foreach (var item in list)
            {
                var genres = (await _genreRepository.GetAllAsync()).Where(g => item.genres_id.Contains(g.Id)).ToList();

                if (!genres.Any())
                {
                    return new List<MangaModel>();
                }

                var manga = ConverterModelDTOToModel(item, genres);

                listModels.Add(manga);
            }

            return await _mangaRepository.AddRange(listModels);
        }
        public async override Task<IList<MangaModel>> GetAllAsync()
        {
            var result = await _mangaRepository.GetAllAsync();

            if (!result.Any())
            {
                var message = new
                {
                    result = "The Database doesn't have any manga"
                };
                return new List<MangaModel>();
            }

            foreach (var item in result)
            {
                item.PathToTitlePicture = $"{_localStorage.RelativePath}{item.PathToTitlePicture}";
                foreach (var res in item.PathToFoldersWithGlava)
                {
                    res.LinkToFirstPicture = $"{_localStorage.RelativePath}{res.LinkToFirstPicture}";
                }
            }

            return result;
        }
        public async override Task<MangaModel> GetByIdAsync(string id)
        {
            var result = await _mangaRepository.GetByIdAsync(id);

            result.PathToTitlePicture = $"{_localStorage.RelativePath}{result.PathToTitlePicture}";

            foreach (var res in result.PathToFoldersWithGlava)
            {
                res.LinkToFirstPicture = $"{_localStorage.RelativePath}{res.LinkToFirstPicture}";
            }

            return result;
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

            if (!genres.Any())
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

            manga = ConverterModelDTOToModel(item, genres);

            return await _repository.UpdateAsync(manga);
        }
    }
}
