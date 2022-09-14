using Data.Models.Base;

namespace Repositories.Repositories.Base
{
    public interface IRepository<T> where T : IModel
    {
        Task<IList<T>> GetCertainAmount(int amount);
        Task<IList<T>> AddRange(IList<T> items);
        Task<T> GetByIdAsync(string id);
        Task<T> DeleteAsync(string id);
        Task<IList<T>> GetAllAsync();
        Task<T> CreateAsync(T item);
        Task<T> UpdateAsync(T item);
    }
}
