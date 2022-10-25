using Data.Entities;
using Services.Response;


namespace Services.Wrappers.Base;

public interface IWrapperMangaService : IWrapperResponseService<ResponseModel, MangaEntity>
{
}
