using Data.Entities;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IMangaService : IService<MangaEntity, MangaDTO>
{
    Task<MangaEntity> AddGenresToManga(AddGenreToMangaDTO mangaDTO);
    Task<List<MangaEntity>> FiltrationByDate(string year);
}
