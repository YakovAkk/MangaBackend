using Data.Entities;
using Services.Core.Paginated;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IGenreService
{
    Task<IList<GenreEntity>> FiltrationByName(string name);
    Task<PagedResult<List<GenreEntity>, object>> GetPaginatedGenreList(string sizeOfPage, string page);
    Task<IList<GenreEntity>> AddRange(IList<GenreInput> list);
    Task<GenreEntity> GetByIdAsync(string id);
    Task<IList<GenreEntity>> GetAllAsync();
}
