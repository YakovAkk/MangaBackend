using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IUserService
{
    #region User
    Task<bool> IsUserExists(UserRegistrationDTO userDTO);
    Task<bool> UpdateAsync(UserEntity user);
    Task<UserEntity> GetUserByNameOrEmail(string nameOrEmail);
    Task<IList<UserEntity>> GetAllAsync();
    Task<UserEntity> GetByIdAsync(string user_id);
    #endregion

    #region UsersFavorite
    Task<bool> AddMangaToFavoriteAsync(string userid, string mangaid);
    Task<bool> AddGenreToFavoriteAsync(string userid, string genreid);
    Task<bool> RemoveGenreFromFavoriteAsync(string userid, string genreid);
    Task<bool> RemoveMangaFromFavoriteAsync(string userid, string mangaid);
    Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(string userid);
    Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(string userid);
    #endregion

    #region Auth
    Task<UserEntity> CreateAsync(UserEntity user);
    Task SetRefreshToken(RefreshToken refreshToken, UserEntity user);
    Task SetResetPasswordToken(ResetPasswordToken resetPasswordToken, UserEntity userExist);
    Task VerifyAsync(UserEntity user);
    #endregion
}
