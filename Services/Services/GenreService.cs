using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Services.Base;
using ValidateService.Validate;

namespace Services.Services;

public class GenreService : DbService<AppDBContext>, IGenreService
{
    private readonly IGenreRepository _genreRepository;
    public GenreService(IGenreRepository repository , DbContextOptions<AppDBContext> dbContextOptions) 
        : base(dbContextOptions)
    {
        _genreRepository = repository;
    }
    
    public async Task<IList<GenreEntity>> AddRange(IList<GenreInput> list)
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
