using Data.Entities;
using Microsoft.Extensions.Logging;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.ExtensionMapper;
using Services.Services.Base;

namespace Services.Services;

public class GenreService : BaseService<GenreEntity, GenreDTO>, IGenreService
{
    public GenreService(IGenreRepository repository) : base(repository) 
    {

    }
    public override async Task<GenreEntity> AddAsync(GenreDTO item)
    {
        if (item == null)
        {
            var errorMessage = "The item was null";
            throw new Exception(errorMessage);
        }

        var model = item.toEntity();

        try
        {
            return await _repository.CreateAsync(model);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
    public override async Task<IList<GenreEntity>> AddRange(IList<GenreDTO> list)
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

        return await _repository.AddRange(listModels);
    }
    public override async Task<IList<GenreEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
    public override async Task<GenreEntity> GetByIdAsync(string id)
    {
        if (String.IsNullOrEmpty(id))
        {
            var errorMessage = "Id was null or empty";
            throw new Exception(errorMessage);
        }

        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
    public async override Task<GenreEntity> UpdateAsync(GenreDTO item)
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

        try
        {
            var genre = await _repository.GetByIdAsync(item.Id);

            genre = item.toEntity();

            return await _repository.UpdateAsync(genre);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
