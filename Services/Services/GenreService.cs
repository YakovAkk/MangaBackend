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
    private readonly ILogger<GenreService> _logger;
    private readonly ILogsTool _logTool;
    public GenreService(IGenreRepository repository, ILogger<GenreService> logger, ILogsTool tool ) : base(repository, logger,tool) 
    {
        _logger = logger;
        _logTool = tool;
    }
    public override async Task<GenreEntity> AddAsync(GenreDTO item)
    {
        _logTool.NameOfMethod = nameof(AddAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"item = {item}");

        if (item == null)
        {
            var errorMessage = "The item was null";
            _logTool.WriteToLog(_logger, LogPosition.Error, $"{errorMessage}");
            throw new Exception(errorMessage);
        }

        var model = item.toEntity();

        try
        {
            return await _repository.CreateAsync(model);
        }
        catch (Exception ex)
        {
            _logTool.WriteToLog(_logger, LogPosition.Error, $"{ex.Message}");
            throw new Exception(ex.Message);
        }
        
    }
    public override async Task<IList<GenreEntity>> AddRange(IList<GenreDTO> list)
    {
        _logTool.NameOfMethod = nameof(AddRange);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"{list}");

        if (list == null)
        {
            var errorMessage = "list was null";

            _logTool.WriteToLog(_logger, LogPosition.Error, $"{errorMessage}");
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
        _logTool.NameOfMethod = nameof(GetAllAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin);

        return await _repository.GetAllAsync();
    }
    public override async Task<GenreEntity> GetByIdAsync(string id)
    {
        _logTool.NameOfMethod = nameof(GetByIdAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {id}");

        if (String.IsNullOrEmpty(id))
        {
            var errorMessage = "Id was null or empty";

            _logTool.WriteToLog(_logger, LogPosition.Error, $"{errorMessage}");
            throw new Exception(errorMessage);
        }

        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logTool.WriteToLog(_logger, LogPosition.Error, $"{ex.Message}");
            throw new Exception(ex.Message);
        }
        
    }
    public async override Task<GenreEntity> UpdateAsync(GenreDTO item)
    {
        _logTool.NameOfMethod = nameof(UpdateAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"{item}");

        if (item == null)
        {
            var errorMessage = "item was null or empty";

            _logTool.WriteToLog(_logger, LogPosition.Error, $"{errorMessage}");
            throw new Exception(errorMessage);
        }

        if (String.IsNullOrEmpty(item.Id))
        {
            var errorMessage = "Id was null or empty";

            _logTool.WriteToLog(_logger, LogPosition.Error, $"{errorMessage}");

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
            _logTool.WriteToLog(_logger, LogPosition.Error, $"{ex.Message}");

            throw new Exception(ex.Message);
        }
    }
}
