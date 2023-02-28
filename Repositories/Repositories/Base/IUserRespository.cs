using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;

namespace Repositories.Repositories.Base;
public interface IUserRespository
{
    Task<UserEntity> CreateAsync(UserEntity user);
    Task SetRefreshToken(RefreshToken refreshToken, UserEntity user);
    Task SetResetPasswordToken(ResetPasswordToken resetPasswordToken, UserEntity userExist);
    Task VerifyAsync(UserEntity user);
}
