using Data.Entities;
using Services.Response;

namespace Services.Wrappers.Base;

public interface IWrapperGenreService : IWrapperResponseService<ResponseModel, GenreEntity>
{
}
