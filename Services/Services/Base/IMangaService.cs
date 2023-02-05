using Data.Entities;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IMangaService
{
    Task<IList<MangaEntity>> FiltrationByName(string name);
    Task<IList<MangaEntity>> GetCertainPage(string sizeOfPage, string page);
    Task<IList<MangaEntity>> AddRange(IList<MangaDTO> list);
    Task<MangaEntity> AddAsync(MangaDTO item);
    Task<MangaEntity> UpdateAsync(MangaDTO item);
    Task<MangaEntity> DeleteAsync(string id);
    Task<MangaEntity> GetByIdAsync(string id);
    Task<IList<MangaEntity>> GetAllAsync();
    Task<MangaEntity> AddGenresToManga(AddGenreToMangaDTO mangaDTO);
    Task<List<MangaEntity>> FiltrationByDate(string year);
}
