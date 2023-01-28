using Data.Entities.Base;
using Repositories.Repositories.Base;
using Services.Model.DTO.Base;
using ValidateService.Validate;

namespace Services.Services.Base;

public abstract class BaseService<TR, TI> : IService<TR, TI>
    where TR : IEntity
    where TI : IModelDTO
{
    protected IRepository<TR> _repository;

    protected BaseService(IRepository<TR> repository)
    {
        _repository = repository;
    }

    public abstract Task<IList<TR>> AddRange(IList<TI> list);
    public abstract Task<IList<TR>> GetAllAsync();
    public abstract Task<TR> GetByIdAsync(string id);
    public abstract Task<TR> AddAsync(TI item);
    public abstract Task<TR> UpdateAsync(TI item);
    public async virtual Task<TR> DeleteAsync(string id)
    {
        try
        {
            if (!String.IsNullOrEmpty(id))
            {
                var errorMessage = "Id was null or empty";
                throw new Exception(errorMessage);
            }

            return await _repository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async virtual Task<IList<TR>> GetCertainPage(string sizeOfPage, string page)
    {
        int pageSize, numberOfPage;

        if (!ValidatorService.IsValidPageAndPageSize(sizeOfPage, page, out pageSize, out numberOfPage))
        {
            throw new Exception("Parameters aren't valid");
        }

        try
        {
            return await _repository.GetCertainPage(pageSize, numberOfPage);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async virtual Task<IList<TR>> FiltrationByName(string name)
    {
        if (String.IsNullOrEmpty(name))
        {
            var errorMessage = "Name was null or empty!";
            throw new Exception(errorMessage);
        }

        return await _repository.FiltrationByName(name);
    }
}
