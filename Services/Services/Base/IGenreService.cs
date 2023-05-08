using Data.Entities;
using Services.Core.Paginated;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IGenreService
{
    Task<IList<GenreEntity>> FiltrationByName(string name);
    Task<PagedResult<List<GenreEntity>, object>> GetPaginatedGenreList(int sizeOfPage, int page);
    Task<IList<GenreEntity>> AddRange(IList<GenreInput> list);
    Task<GenreEntity> GetByIdAsync(int id);
    Task<IList<GenreEntity>> GetAllAsync();
}
