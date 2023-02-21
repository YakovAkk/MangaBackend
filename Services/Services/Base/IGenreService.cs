using Data.Entities;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IGenreService
{
    Task<IList<GenreEntity>> FiltrationByName(string name);
    Task<IList<GenreEntity>> GetCertainPage(string sizeOfPage, string page);
    Task<IList<GenreEntity>> AddRange(IList<GenreInput> list);
    Task<GenreEntity> GetByIdAsync(string id);
    Task<IList<GenreEntity>> GetAllAsync();
}
