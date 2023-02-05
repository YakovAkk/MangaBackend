using Data.Entities;

namespace Repositories.Repositories.Base;

public interface IGenreRepository
{
    Task<IList<GenreEntity>> GetCertainPage(int sizeOfPage, int page);
    Task<IList<GenreEntity>> AddRange(IList<GenreEntity> items);
    Task<GenreEntity> GetByIdAsync(string id);
    Task<GenreEntity> DeleteAsync(string id);
    Task<IList<GenreEntity>> GetAllAsync();
    Task<GenreEntity> CreateAsync(GenreEntity item);
    Task<GenreEntity> UpdateAsync(GenreEntity item);
    Task<IList<GenreEntity>> FiltrationByName(string name);
}
