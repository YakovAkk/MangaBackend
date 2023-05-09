using Data.Entities;
using Services.Core.Paginated;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IGenreService
{
    Task<List<GenreEntity>> FiltrationByName(string name);
    Task<PagedResult<List<GenreEntity>, object>> GetPaginatedGenreList(int sizeOfPage, int page);
    Task<List<GenreEntity>> AddRange(List<GenreInput> list);
    Task<GenreEntity> GetByIdAsync(int id);
    Task<List<GenreEntity>> GetAllAsync();
    Task<bool> IsGenreExist(int genreId);
}
