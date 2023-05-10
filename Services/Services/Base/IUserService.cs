using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;
using Services.Model.InputModel;

namespace Services.Services.Base;

public interface IUserService
{
    #region User
    Task<bool> IsUserExistsAsync(string email, string name);
    Task<bool> UpdateUserAsync(UserInputModel userInputModel);
    Task<UserEntity> GetUserByNameAsync(string name);
    Task<UserEntity> GetUserByEmailAsync(string email);
    Task<List<UserEntity>> GetAllAsync();
    Task<UserEntity> GetByIdAsync(int user_id);
    #endregion

    #region UsersFavorite
    Task<bool> AddMangaToFavoriteAsync(int userid, int mangaid);
    Task<bool> AddGenreToFavoriteAsync(int userid, int genreid);
    Task<bool> RemoveGenreFromFavoriteAsync(int userid, int genreid);
    Task<bool> RemoveMangaFromFavoriteAsync(int userid, int mangaid);
    Task<List<MangaEntity>> GetAllFavoriteMangaAsync(int userid);
    Task<List<GenreEntity>> GetAllFavoriteGenreAsync(int userid);
    #endregion

    #region Auth
    Task<UserEntity> CreateAsync(UserEntity user);
    Task SetRefreshToken(RefreshToken refreshToken, UserEntity user);
    Task SetResetPasswordToken(ResetPasswordToken resetPasswordToken, UserEntity userExist);
    Task SetVerivicationAsync(UserEntity user);
    #endregion
}
