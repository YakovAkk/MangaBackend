using AutoMapper;
using Data.Database;
using Data.Models.Base;
using Repositories.Models.Base;

namespace Repositories.Repositories.Base
{
    public abstract class BaseRepository<T> : IRepository<T> where T : IModel
    {
        protected AppDBContent _db { get; set; }
        protected IMapper _mapper;
        public BaseRepository(AppDBContent db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public abstract Task<IList<T>> GetAllAsync();
        public abstract Task<T> CreateAsync(T item);
        public abstract Task<T> DeleteAsync(string id);
        public abstract Task<T> GetByIdAsync(string id);
        public abstract Task<T> UpdateAsync(T item);
        public abstract Task<IList<T>> AddRange(IList<T> items);
        public abstract Task<IList<T>> GetAllFavoriteAsync();
        public abstract Task<T> AddToFavorite(string Id);
        public abstract Task<T> RemoveFavorite(string Id);
        public abstract Task<IList<T>> GetCertainPage(int sizeOfPage, int page);
    }
}
