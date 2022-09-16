using Data.Models;
using Repositories.Models;
using Services.Response;

namespace Services.Wrappers.Base
{
    public interface IWrapperGenreService : IWrapperResponseService<ResponseModel, GenreModel>
    {
    }
}
