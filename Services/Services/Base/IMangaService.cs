using Data.Models;
using Services.DTO;

namespace Services.Services.Base
{
    public interface IMangaService : IService<MangaEntity, MangaDTO>
    {
        Task<MangaEntity> AddGenresToManga(AddGenreToMangaDTO mangaDTO);
        Task<List<MangaEntity>> FiltrationByDate(int year);
    }
}
