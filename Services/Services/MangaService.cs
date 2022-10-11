using Data.Entities;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.ExtensionMapper;
using Services.Services.Base;
using Services.Storage.Base;

namespace Services.Services
{
    public class MangaService : BaseService<MangaEntity, MangaDTO>, IMangaService
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

        public override async Task<MangaEntity> AddAsync(MangaDTO item)
        {
            if (item == null)
            {
                throw new Exception("The item was null");
            }

            var genres = (await _genreRepository.GetAllAsync()).Where(g => item.genres_id.Contains(g.Id)).ToList();

            if (!genres.Any())
            {
                throw new Exception("The database doesn't contain the genres");
            }

            var model = item.toEntity(genres);

            try
            {
                return await _repository.CreateAsync(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<MangaEntity> AddGenresToManga(AddGenreToMangaDTO mangaDTO)
        {
            if (mangaDTO == null)
            {
                throw new Exception("The item was null");
            }

            var manga = await _mangaRepository.GetByIdAsync(mangaDTO.MangaId);

            if (manga == null)
            {
                throw new Exception("The manga isn't contained in the database!");
            }

            var allGenres = await _genreRepository.GetAllAsync();

            var genres = allGenres.Where(g => mangaDTO.Genres_id.Contains(g.Id));

            if (!genres.Any())
            {
                throw new Exception("The genres are incorrect");
            }

            manga.Genres.AddRange(genres);

            try
            {
                var res = await _mangaRepository.UpdateAsync(manga);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }  
        }
        public async override Task<IList<MangaEntity>> AddRange(IList<MangaDTO> list)
        {
            if(list == null)
            {
                throw new Exception("The list was null");
            }

            var listModels = new List<MangaEntity>();

            foreach (var item in list)
            {
                var genres = (await _genreRepository.GetAllAsync()).Where(g => item.genres_id.Contains(g.Id)).ToList();

                if (!genres.Any())
                {
                    return new List<MangaEntity>();
                }

                var manga = item.toEntity(genres);

                listModels.Add(manga);
            }

            try
            {
                return await _mangaRepository.AddRange(listModels);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public async override Task<IList<MangaEntity>> GetAllAsync()
        {
            var result = await _mangaRepository.GetAllAsync();

            if (!result.Any())
            {
                var message = new
                {
                    result = "The Database doesn't have any manga"
                };
                return new List<MangaEntity>();
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
        public async override Task<MangaEntity> GetByIdAsync(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new Exception("Id was null or empty");
            }
            try
            {
                var result = await _mangaRepository.GetByIdAsync(id);

                result.PathToTitlePicture = $"{_localStorage.RelativePath}{result.PathToTitlePicture}";

                foreach (var res in result.PathToFoldersWithGlava)
                {
                    res.LinkToFirstPicture = $"{_localStorage.RelativePath}{res.LinkToFirstPicture}";
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public async override Task<MangaEntity> UpdateAsync(MangaDTO item)
        {
            if(item == null)
            {
                throw new Exception("The item was null");
            }

            if (String.IsNullOrEmpty(item.Id))
            {
                throw new Exception("Id was null or empty");
            }

            var genres = (await _genreRepository.GetAllAsync()).Where(g => item.genres_id.Contains(g.Id)).ToList();

            if (!genres.Any())
            {
                throw new Exception("The database doesn't contain any genres");
            }

            try
            {
                var manga = await _repository.GetByIdAsync(item.Id);
                manga = item.toEntity(genres);

                return await _repository.UpdateAsync(manga);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<MangaEntity>> FiltrationByDate(int year)
        {
            return await _mangaRepository.FiltrationByDate(year);
        }
    }
}
