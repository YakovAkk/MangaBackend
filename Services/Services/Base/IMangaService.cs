using Data.Entities;
using Services.Core.Paginated;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IMangaService
{
    Task<List<MangaEntity>> FiltrationByNameAsync(string name);
    Task<PagedResult<List<MangaEntity>, object>> GetPagiantedMangaListAsync(int sizeOfPage, int page);
    Task<List<MangaEntity>> AddRangeAsync(List<MangaInput> list);
    Task<MangaEntity> GetByIdAsync(int id);
    Task<List<MangaEntity>> GetAllAsync();
    Task<List<MangaEntity>> FiltrationByDateAsync(int year);
    Task<bool> IsMangaExistAsync(int mangaId);
}
