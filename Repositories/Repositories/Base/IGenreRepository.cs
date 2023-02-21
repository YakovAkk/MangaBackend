using Data.Entities;

namespace Repositories.Repositories.Base;

public interface IGenreRepository
{
    Task<GenreEntity> GetByIdAsync(string id);
    Task<IList<GenreEntity>> GetAllAsync();
}
