using Data.Entities;

namespace Repositories.Repositories.Base;

public interface IMangaRepository
{
    Task<IList<MangaEntity>> GetCertainPage(int sizeOfPage, int page);
    Task<IList<MangaEntity>> AddRange(IList<MangaEntity> items);
    Task<MangaEntity> GetByIdAsync(string id);
    Task<MangaEntity> DeleteAsync(string id);
    Task<IList<MangaEntity>> GetAllAsync();
    Task<MangaEntity> CreateAsync(MangaEntity item);
    Task<MangaEntity> UpdateAsync(MangaEntity item);
    Task<IList<MangaEntity>> FiltrationByName(string name);
    Task<List<MangaEntity>> FiltrationByDate(int year);
}
