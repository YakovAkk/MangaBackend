using Data.Database;
using Data.Models.Base;

namespace Repositories.Repositories.Base
{
    public abstract class BaseRepository<T> : IRepository<T> where T : IModel
    {
        protected AppDBContent _db { get; set; }
        public BaseRepository(AppDBContent db)
        {
            _db = db;
        }
        public abstract Task<List<T>> GetAllAsync();
        public abstract Task<T> CreateAsync(T item);
        public abstract Task<T> DeleteAsync(string id);
        public abstract Task<T> GetByIdAsync(string id);
        public abstract Task<T> UpdateAsync(T item);
    }
}
