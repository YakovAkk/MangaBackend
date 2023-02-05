using Data.Entities;
using Repositories.Repositories.Base;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Services.Base;
using Services.Storage.Base;
using ValidateService.Validate;

namespace Services.Services;

public class MangaService : IMangaService
{
    private readonly ILocalStorage _localStorage;
    private readonly IGenreRepository _genreRepository;
    private readonly IMangaRepository _mangaRepository;
    public MangaService(
        IMangaRepository repository,
        IGenreRepository genreRepository, 
        ILocalStorage localStorage)
    {
        _genreRepository = genreRepository;
        _mangaRepository = repository;
        _localStorage = localStorage;
    }

    public async Task<MangaEntity> AddAsync(MangaDTO item)
    {
        if (item == null)
        {
            var errorMessage = "The item was null";
            throw new Exception(errorMessage);
        }

        var allGenres = await _genreRepository.GetAllAsync();

        var genres = allGenres.Where(g => item.Genres_id.Contains(g.Id)).ToList();

        if (!genres.Any())
        {
            var errorMessage = "The database doesn't contain the genres";
            throw new Exception(errorMessage);
        }

        var model = item.toEntity(genres);

        try
        {
            return await _mangaRepository.CreateAsync(model);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<MangaEntity> AddGenresToManga(AddGenreToMangaDTO mangaDTO)
    {
        if (mangaDTO == null || mangaDTO?.MangaId == null)
        {
            var errorMessage = "The item was null";
            throw new Exception(errorMessage);
        }

        var manga = await _mangaRepository.GetByIdAsync(mangaDTO.MangaId);

        if (manga == null)
        {
            var errorMessage = "The manga isn't contained in the database!";
            throw new Exception(errorMessage);
        }

        var allGenres = await _genreRepository.GetAllAsync();

        var genres = allGenres.Where(g => mangaDTO.Genres_id.Contains(g.Id));

        if (!genres.Any())
        {
            var errorMessage = "The genres are incorrect";
           
            throw new Exception(errorMessage);
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
    public async Task<IList<MangaEntity>> AddRange(IList<MangaDTO> list)
    {
        if (list == null)
        {
            var errorMessage = "The list was null";
            throw new Exception(errorMessage);
        }

        var listModels = new List<MangaEntity>();

        foreach (var item in list)
        {
            var allGenres = await _genreRepository.GetAllAsync();

            var genres = allGenres.Where(g => item.Genres_id.Contains(g.Id)).ToList();

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
    public async Task<IList<MangaEntity>> GetAllAsync()
    {
        var result = await _mangaRepository.GetAllAsync();

        if (!result.Any())
        {
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
    public async Task<MangaEntity> GetByIdAsync(string Id)
    {
        if (String.IsNullOrEmpty(Id))
        {
            var errorMessage = "Id was null or empty";
         
            throw new Exception(errorMessage);
        }
        try
        {
            var result = await _mangaRepository.GetByIdAsync(Id);

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
    public async Task<MangaEntity> UpdateAsync(MangaDTO item)
    {
       
        if (item == null)
        {
            var errorMessage = "The item was null";
          
            throw new Exception(errorMessage);
        }

        if (String.IsNullOrEmpty(item.Id))
        {
             var errorMessage = "Id was null or empty";
             throw new Exception(errorMessage);
        }

        var allGenres = await _genreRepository.GetAllAsync();

        var genres = allGenres.Where(g => item.Genres_id.Contains(g.Id)).ToList();

        if (!genres.Any())
        {
            var errorMessage = "The database doesn't contain any genres";
            
            throw new Exception(errorMessage);
        }

        try
        {
            var manga = await _mangaRepository.GetByIdAsync(item.Id);
            manga = item.toEntity(genres);

            return await _mangaRepository.UpdateAsync(manga);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<MangaEntity>> FiltrationByDate(string year)
    {
        int yearnum = 0;

        if (!ValidatorService.IsValidYear(year, out yearnum))
        {
            throw new Exception("Parameters aren't valid");
        }

        try
        {
            return await _mangaRepository.FiltrationByDate(yearnum);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<MangaEntity> DeleteAsync(string id)
    {
        try
        {
            if (!String.IsNullOrEmpty(id))
            {
                var errorMessage = "Id was null or empty";
                throw new Exception(errorMessage);
            }

            return await _mangaRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<IList<MangaEntity>> GetCertainPage(string sizeOfPage, string page)
    {
        int pageSize, numberOfPage;

        if (!ValidatorService.IsValidPageAndPageSize(sizeOfPage, page, out pageSize, out numberOfPage))
        {
            throw new Exception("Parameters aren't valid");
        }

        try
        {
            return await _mangaRepository.GetCertainPage(pageSize, numberOfPage);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async Task<IList<MangaEntity>> FiltrationByName(string name)
    {
        if (String.IsNullOrEmpty(name))
        {
            var errorMessage = "Name was null or empty!";
            throw new Exception(errorMessage);
        }

        return await _mangaRepository.FiltrationByName(name);
    }
}
