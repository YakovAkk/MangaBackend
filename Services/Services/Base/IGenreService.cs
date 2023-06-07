using Data.Database;
using Data.Entities;
using Services.Core.Paginated;
using Services.Model.InputModel;

namespace Services.Services.Base;

public interface IGenreService
{
    Task<List<GenreEntity>> FiltrationByNameAsync(string name);
    Task<PagedResult<List<GenreEntity>, object>> GetPaginatedGenreListAsync(int sizeOfPage, int page);
    Task<List<GenreEntity>> AddRangeAsync(List<GenreInputModel> list);
    Task<GenreEntity> GetByIdAsync(int id);
    Task<List<GenreEntity>> GetAllAsync();
    Task<bool> IsGenreExistAsync(int genreId);

    #region Internal
    IQueryable<GenreEntity> GetRangeByIdInternalAsync(List<int> recomendedGenresIds, AppDBContext context);
    #endregion
}
