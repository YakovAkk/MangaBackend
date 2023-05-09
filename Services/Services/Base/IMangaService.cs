using Data.Entities;
using Services.Core.Paginated;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IMangaService
{
    Task<List<MangaEntity>> FiltrationByName(string name);
    Task<PagedResult<List<MangaEntity>, object>> GetPagiantedMangaList(int sizeOfPage, int page);
    Task<List<MangaEntity>> AddRange(List<MangaInput> list);
    Task<MangaEntity> GetByIdAsync(int id);
    Task<List<MangaEntity>> GetAllAsync();
    Task<List<MangaEntity>> FiltrationByDate(int year);
    Task<bool> IsMangaExist(int mangaId);
}
