using Data.Entities.Base;
using Microsoft.Extensions.Logging;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Repositories.Repositories.Base;
using Services.DTO.Base;

namespace Services.Services.Base;

public abstract class BaseService<TR, TI> : IService<TR, TI>
    where TR : IEntity
    where TI : IModelDTO
{
    protected IRepository<TR> _repository;
    private readonly ILogger<GenreService> _logger;
    private readonly ITool _logTool;

    protected BaseService(IRepository<TR> repository, ILogger<GenreService> logger, ITool logTool)
    {
        _repository = repository;
        _logger = logger;
        _logTool = logTool;
    }

    public abstract Task<IList<TR>> AddRange(IList<TI> list);
    public abstract Task<IList<TR>> GetAllAsync();
    public abstract Task<TR> GetByIdAsync(string id);
    public abstract Task<TR> AddAsync(TI item);
    public abstract Task<TR> UpdateAsync(TI item);
    public async virtual Task<TR> DeleteAsync(string id)
    {
        _logTool.NameOfMethod = nameof(DeleteAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {id}");

        try
        {
            if (!String.IsNullOrEmpty(id))
            {
                var errorMessage = "Id was null or empty";
                _logTool.WriteToLog(_logger, LogPosition.Error, $"{errorMessage}");
                throw new Exception(errorMessage);
            }

            return await _repository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logTool.WriteToLog(_logger, LogPosition.Error, $"{ex.Message}");
            throw new Exception(ex.Message);
        }
    }
    public async virtual Task<IList<TR>> GetCertainPage(int sizeOfPage, int page)
    {
        _logTool.NameOfMethod = nameof(GetCertainPage);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"sizeOfPage = {sizeOfPage} page = {page}");

        try
        {
            return await _repository.GetCertainPage(sizeOfPage, page);
        }
        catch (Exception ex)
        {
            _logTool.WriteToLog(_logger, LogPosition.Error, ex.Message);
            throw new Exception(ex.Message);
        }

    }
    public async virtual Task<IList<TR>> FiltrationByName(string name)
    {
        _logTool.NameOfMethod = nameof(FiltrationByName);
        _logTool.WriteToLog(_logger, LogPosition.Begin);

        if (String.IsNullOrEmpty(name))
        {
            var errorMessage = "Name was null or empty!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);
            throw new Exception(errorMessage);
        }

        return await _repository.FiltrationByName(name);
    }
}
