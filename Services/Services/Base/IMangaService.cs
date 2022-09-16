using Data.Models;
using Repositories.Models;
using Services.DTO;

namespace Services.Services.Base
{
    public interface IMangaService : IService<MangaModel, MangaDTO>
    {
        Task<MangaModel> AddGenresToManga(AddGenreToMangaDTO mangaDTO);
    }
}
