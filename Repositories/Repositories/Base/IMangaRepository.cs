using Data.Models;

namespace Repositories.Repositories.Base
{
    public interface IMangaRepository : IRepository<MangaEntity>
    {
        Task<List<MangaEntity>> FiltrationByDate(int year);
    }
}
