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
        var model = item.toEntity();

        return await _genreRepository.CreateAsync(model);
    }
    public async Task<IList<GenreEntity>> AddRange(IList<GenreDTO> list)
    {
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
        return await _genreRepository.GetByIdAsync(id);
    }
    public async Task<GenreEntity> UpdateAsync(GenreDTO item)
    {
        var genre = await _genreRepository.GetByIdAsync(item.Id);

        genre = item.toEntity();

        return await _genreRepository.UpdateAsync(genre);
    }
    public async Task<GenreEntity> DeleteAsync(string id)
    {
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
        return await _genreRepository.FiltrationByName(name);
    }
}
