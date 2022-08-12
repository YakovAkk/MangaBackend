using Data.Models.Base;
using Services.DTO.Base;

namespace Services.Services.Base
{
    public interface IService<TReturn, TInput> where TReturn : IModel where TInput : IModelDTO
    {
        Task<TReturn> AddAsync(TInput item);
        Task<TReturn> UpdateAsync(TInput item);
        Task<TReturn> DeleteAsync(string id);
        Task<TReturn> GetByIdAsync(string id);
        Task<List<TReturn>> GetAllAsync();
    }
}
