using Data.Models;

namespace Repositories.Repositories.Base
{
    public interface IMangaRepository : IRepository<MangaModel>
    {
        Task<List<MangaModel>> FiltrationByDate(int year);
    }
}
