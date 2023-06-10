using Data.Database;
using Data.Entities;
using Services.Core.Paginated;
using Services.Model.InputModel;
using Services.Model.ViewModel;

namespace Services.Services.Base;

public interface IMangaService
{
    Task<List<MangaEntity>> FiltrationByNameAsync(string name);
    Task<PagedResult<List<MangaEntity>, object>> GetPagiantedMangaListAsync(int sizeOfPage, int page);
    Task<List<MangaViewModel>> AddRangeAsync(List<MangaInputModel> list);
    Task<MangaEntity> GetByIdAsync(int id);
    Task<List<MangaViewModel>> GetAllAsync();
    Task<List<MangaEntity>> FiltrationByDateAsync(int year);
    Task<bool> IsMangaExistAsync(int mangaId);

    #region Internal
    IQueryable<MangaEntity> GetRangeByIdInternalAsync(List<int> sharedMangasIds, AppDBContext context);
    IQueryable<MangaEntity> GetAllInternalAsync(AppDBContext context);
    #endregion
}
