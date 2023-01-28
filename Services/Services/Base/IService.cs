using Data.Entities.Base;
using Services.DTO.Base;

namespace Services.Services.Base;

public interface IService<TReturn, TInput> 
    where TReturn : IModel 
    where TInput : IModelDTO
{
    Task<IList<TReturn>> FiltrationByName(string name);
    Task<TReturn> RemoveFavorite(string Id);
    Task<TReturn> AddToFavorite(string Id);
    Task<IList<TReturn>> GetAllFavoriteAsync();
    Task<IList<TReturn>> GetCertainPage(string sizeOfPage, string page);
    Task<IList<TReturn>> AddRange(IList<TInput> list);
    Task<TReturn> AddAsync(TInput item);
    Task<TReturn> UpdateAsync(TInput item);
    Task<TReturn> DeleteAsync(string id);
    Task<TReturn> GetByIdAsync(string id);
    Task<IList<TReturn>> GetAllAsync();
}
