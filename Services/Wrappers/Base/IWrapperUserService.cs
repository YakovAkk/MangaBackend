using Data.Entities;
using Services.Response;

namespace Services.Wrappers.Base;

public interface IWrapperUserService : IWrapperResponseService<ResponseModel, UserEntity>
{
}
