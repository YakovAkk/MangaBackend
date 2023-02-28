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
    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        var userAdded = await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        if(userAdded == null)
        {
            var errorMessage = "User hasn't been added";
         
            throw new Exception(errorMessage);  
        }
        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Name == user.Name);

        if(userResult == null)
        {
            var errorMessage = "User hasn't been added";
        
            throw new Exception(errorMessage);
        }
        return userResult;
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
