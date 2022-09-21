using Data.Models;
using Services.DTO;

namespace Services.Services.Base
{
    public interface IMangaService : IService<MangaModel, MangaDTO>
    {
        Task<MangaModel> AddGenresToManga(AddGenreToMangaDTO mangaDTO);
        Task<List<MangaModel>> FiltrationByDate(int year);
    }
}
