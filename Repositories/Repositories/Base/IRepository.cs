using Data.Models.Base;

namespace Repositories.Repositories.Base
{
    public interface IRepository<T> where T : IModel
    {
        Task<T> GetByIdAsync(string id);
        Task<T> DeleteAsync(string id);
        Task<List<T>> GetAllAsync();
        Task<T> CreateAsync(T item);
        Task<T> UpdateAsync(T item);
    }
}
