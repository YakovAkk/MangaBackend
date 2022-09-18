using AutoMapper;
using Data.Models.Base;
using Repositories.Models.Base;
using Repositories.Repositories.Base;
using Services.DTO.Base;

namespace Services.Services.Base
{
    public abstract class BaseService<TR, TI> : IService<TR, TI> 
        where TR : IModel 
        where TI : IModelDTO
    {
        protected IRepository<TR> _repository;
        protected IMapper _mapper;
        public BaseService(IRepository<TR> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public abstract Task<IList<TR>> AddRange(IList<TR> list);
        public abstract Task<IList<TR>> GetAllAsync();
        public abstract Task<TR> GetByIdAsync(string id);
        public abstract Task<TR> AddAsync(TI item);
        public abstract Task<TR> UpdateAsync(TI item);
        public async virtual Task<TR> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async virtual Task<IList<TR>> GetCertainPage(int sizeOfPage, int page)
        {
            return await _repository.GetCertainPage(sizeOfPage, page);
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
