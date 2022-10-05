using Data.Entities.Base;

namespace Repositories.Repositories.Base
{
    public interface IRepository<T> where T : IModel
    {
        Task<T> RemoveFavorite(string Id);
        Task<T> AddToFavorite(string Id);
        Task<IList<T>> GetAllFavoriteAsync();
        Task<IList<T>> GetCertainPage(int sizeOfPage, int page);
        Task<IList<T>> AddRange(IList<T> items);
        Task<T> GetByIdAsync(string id);
        Task<T> DeleteAsync(string id);
        Task<IList<T>> GetAllAsync();
        Task<T> CreateAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<IList<T>> FiltrationByName(string name);
    }
}
