using Data.Database;
using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Repositories.Repositories;
public class UserRepository : IUserRespository
{
    private readonly AppDBContext _db;
    public UserRepository(AppDBContext db)
    {
        _db = db;
    }
    public async Task SetRefreshToken(RefreshToken refreshToken, UserEntity user)
    {
        user.RefreshToken = refreshToken.Token;
        user.TokenExpires = refreshToken.Expires;
        user.TokenCreated = refreshToken.Created;

        await _db.SaveChangesAsync();
    }
    public async Task VerifyAsync(UserEntity user)
    {
        user.VerifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
    public async Task SetResetPasswordToken(ResetPasswordToken resetPasswordToken, UserEntity userExist)
    {
        userExist.ResetPasswordToken = resetPasswordToken.Token;
        userExist.ResetPasswordTokenExpires = resetPasswordToken.Expires;

        await _db.SaveChangesAsync();
    }
}
