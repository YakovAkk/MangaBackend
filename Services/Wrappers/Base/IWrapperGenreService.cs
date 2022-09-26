using Data.Models;
using Services.Response;

namespace Services.Wrappers.Base
{
    public interface IWrapperGenreService : IWrapperResponseService<ResponseModel, GenreEntity>
    {
    }
}
