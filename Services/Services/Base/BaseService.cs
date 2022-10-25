using Data.Entities.Base;
using Repositories.Repositories.Base;
using Services.DTO.Base;

namespace Services.Services.Base;

public abstract class BaseService<TR, TI> : IService<TR, TI> 
    where TR : IModel 
    where TI : IModelDTO
{
    protected IRepository<TR> _repository;

    public BaseService(IRepository<TR> repository)
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
                throw new Exception("Id was null or empty");
            }

            return await _repository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async virtual Task<IList<TR>> GetCertainPage(int sizeOfPage, int page)
    {
        try
        {
            return await _repository.GetCertainPage(sizeOfPage, page);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
    public async virtual Task<IList<TR>> GetAllFavoriteAsync()
    {
        try
        {
            return await _repository.GetAllFavoriteAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async virtual Task<TR> AddToFavorite(string Id)
    {
        try
        {
            return await _repository.AddToFavorite(Id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async virtual Task<TR> RemoveFavorite(string Id)
    {
        try
        {
            return await _repository.RemoveFavorite(Id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async virtual Task<IList<TR>> FiltrationByName(string name)
    {
        try
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new Exception("Name was null or empty!");
            } 
            return await _repository.FiltrationByName(name);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
