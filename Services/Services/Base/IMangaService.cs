using Data.Entities;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IMangaService
{
    Task<IList<MangaEntity>> FiltrationByName(string name);
    Task<IList<MangaEntity>> GetCertainPage(string sizeOfPage, string page);
    Task<IList<MangaEntity>> AddRange(IList<MangaInput> list);
    Task<MangaEntity> GetByIdAsync(string id);
    Task<IList<MangaEntity>> GetAllAsync();
    Task<List<MangaEntity>> FiltrationByDate(string year);
}
