using Data.Entities;
using Repositories.Repositories.Base;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Services.Base;
using ValidateService.Validate;

namespace Services.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    public GenreService(IGenreRepository repository)
    {
        _genreRepository = repository;
    }
    public async Task<GenreEntity> AddAsync(GenreDTO item)
    {
        if (item == null)
        {
            var errorMessage = "The item was null";
            throw new Exception(errorMessage);
        }

        var model = item.toEntity();

        return await _genreRepository.CreateAsync(model);
    }
    public async Task<IList<GenreEntity>> AddRange(IList<GenreDTO> list)
    {
        if (list == null)
        {
            var errorMessage = "list was null";
            throw new Exception(errorMessage);
        }

        var listModels = new List<GenreEntity>();

        foreach (var item in list)
        {
            var genre = item.toEntity();

            listModels.Add(genre);
        }

        return await _genreRepository.AddRange(listModels);
    }
    public async Task<IList<GenreEntity>> GetAllAsync()
    {
        return await _genreRepository.GetAllAsync();
    }
    public async Task<GenreEntity> GetByIdAsync(string id)
    {
        if (String.IsNullOrEmpty(id))
        {
            var errorMessage = "Id was null or empty";
            throw new Exception(errorMessage);
        }

        return await _genreRepository.GetByIdAsync(id);
    }
    public async Task<GenreEntity> UpdateAsync(GenreDTO item)
    {
        if (item == null)
        {
            var errorMessage = "item was null or empty";
            throw new Exception(errorMessage);
        }

        if (String.IsNullOrEmpty(item.Id))
        {
            var errorMessage = "Id was null or empty";

            throw new Exception(errorMessage);
        }

        var genre = await _genreRepository.GetByIdAsync(item.Id);

        genre = item.toEntity();

        return await _genreRepository.UpdateAsync(genre);
    }
    public async Task<GenreEntity> DeleteAsync(string id)
    {

        if (!String.IsNullOrEmpty(id))
        {
            var errorMessage = "Id was null or empty";
            throw new Exception(errorMessage);
        }

        return await _genreRepository.DeleteAsync(id);
    }
    public async Task<IList<GenreEntity>> GetCertainPage(string sizeOfPage, string page)
    {
        int pageSize, numberOfPage;

        if (!ValidatorService.IsValidPageAndPageSize(sizeOfPage, page, out pageSize, out numberOfPage))
        {
            throw new Exception("Parameters aren't valid");
        }

        return await _genreRepository.GetCertainPage(pageSize, numberOfPage);
    }
    public async Task<IList<GenreEntity>> FiltrationByName(string name)
    {
        if (String.IsNullOrEmpty(name))
        {
            var errorMessage = "Name was null or empty!";
            throw new Exception(errorMessage);
        }

        return await _genreRepository.FiltrationByName(name);
    }
}
