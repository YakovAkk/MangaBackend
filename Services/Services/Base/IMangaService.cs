using Data.Entities;
using Services.Core.Paginated;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IMangaService
{
    Task<IList<MangaEntity>> FiltrationByName(string name);
    Task<PagedResult<List<MangaEntity>, object>> GetPagiantedMangaList(int sizeOfPage, int page);
    Task<IList<MangaEntity>> AddRange(IList<MangaInput> list);
    Task<MangaEntity> GetByIdAsync(int id);
    Task<IList<MangaEntity>> GetAllAsync();
    Task<List<MangaEntity>> FiltrationByDate(string year);
}
