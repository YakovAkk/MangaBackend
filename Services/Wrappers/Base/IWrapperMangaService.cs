using Data.Models;
using Repositories.Models;
using Services.Response;


namespace Services.Wrappers.Base
{
    public interface IWrapperMangaService : IWrapperResponseService<ResponseModel, MangaModel>
    {
    }
}
