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
        public abstract Task<List<TR>> GetAllAsync();
        public abstract Task<TR> GetByIdAsync(string id);
        public async virtual Task<TR> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
        public abstract Task<TR> AddAsync(TI item);
        public abstract Task<TR> UpdateAsync(TI item);
    }
}
