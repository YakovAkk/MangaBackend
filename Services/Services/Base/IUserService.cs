using Data.Entities;
using Services.Model.Helping;
using Services.Model.InputModel;
using Services.Model.ViewModel;

namespace Services.Services.Base;

public interface IUserService
{
    #region User
    Task<bool> IsUserExistsAsync(string email, string name);
    Task<bool> UpdateUserAsync(UserInputModel userInputModel);
    Task<UserEntity> GetUserByNameAsync(string name);
    Task<UserEntity> GetUserByEmailAsync(string email);
    Task<List<UserViewModel>> GetAllAsync();
    Task<UserEntity> GetByIdAsync(int user_id);
    #endregion

    #region UsersFavorite
    Task<bool> AddMangaToFavoriteAsync(string userId, int mangaId);
    Task<bool> AddGenreToFavoriteAsync(string userId, int genreId);
    Task<bool> RemoveGenreFromFavoriteAsync(string userId, int genreId);
    Task<bool> RemoveMangaFromFavoriteAsync(string userId, int mangaId);
    Task<List<MangaEntity>> GetAllFavoriteMangasAsync(string userId);
    Task<List<GenreEntity>> GetAllFavoriteGenresAsync(string userId);
    #endregion

    #region Auth
    Task<UserEntity> CreateAsync(UserEntity user);
    Task SetRefreshToken(RefreshToken refreshToken, UserEntity user);
    Task SetResetPasswordToken(ResetPasswordToken resetPasswordToken, UserEntity userExist);
    Task SetVerivicationAsync(UserEntity user);
    #endregion

    #region Remember reading
    Task<List<RememberReadingItemViewModel>> GetAllReadingItemsAsync(string userId);
    Task CreateReadingItemAsync(string userId, RememberReadingItemInputModel inputModel);
    Task<RememberReadingItemViewModel> GetReadingItemAsync(string userId, string mangaId);
    Task<List<MangaViewModel>> GetRecommendationsAsync(string userId);
    #endregion
}
