using Data.Models.Base;
using Repositories.Repositories.Base;
using Services.DTO.Base;

namespace Services.Services.Base
{
    public abstract class BaseService<TR, TI> : IService<TR, TI> where TR : IModel where TI : IModelDTO
    {
        protected IRepository<TR> _repository;

        public BaseService(IRepository<TR> repository)
        {
            _repository = repository;
        }

        public abstract Task<IList<TR>> AddRange(IList<TI> list);
        public abstract Task<IList<TR>> GetAllAsync();
        public abstract Task<TR> GetByIdAsync(string id);
        public async virtual Task<TR> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
        public abstract Task<TR> AddAsync(TI item);
        public abstract Task<TR> UpdateAsync(TI item);
        public async virtual Task<IList<TR>> GetCertainAmount(int amount)
        {
            return await _repository.GetCertainAmount(amount);
        }
        public async virtual Task<IList<TR>> GetAllFavoriteAsync()
        {
            return await _repository.GetAllFavoriteAsync();
        }
        public async virtual Task<TR> AddToFavorite(string Id)
        {
            return await _repository.AddToFavorite(Id);
        }
        public async virtual Task<TR> RemoveFavorite(string Id)
        {
            return await _repository.RemoveFavorite(Id);
        }
    }
}
