using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;
using Services.Model.InputModel;

namespace Services.Services.Base;

public interface IUserService
{
    #region User
    Task<bool> IsUserExists(string email, string name);
    Task<bool> UpdateUserAsync(UserInputModel userInputModel);
    Task<UserEntity> GetUserByNameOrEmail(string nameOrEmail);
    Task<IList<UserEntity>> GetAllAsync();
    Task<UserEntity> GetByIdAsync(int user_id);
    #endregion

    #region UsersFavorite
    Task<bool> AddMangaToFavoriteAsync(int userid, int mangaid);
    Task<bool> AddGenreToFavoriteAsync(int userid, int genreid);
    Task<bool> RemoveGenreFromFavoriteAsync(int userid, int genreid);
    Task<bool> RemoveMangaFromFavoriteAsync(int userid, int mangaid);
    Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(int userid);
    Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(int userid);
    #endregion

    #region Auth
    Task<UserEntity> CreateAsync(UserEntity user);
    Task SetRefreshToken(RefreshToken refreshToken, UserEntity user);
    Task SetResetPasswordToken(ResetPasswordToken resetPasswordToken, UserEntity userExist);
    Task VerifyAsync(UserEntity user);
    #endregion
}
