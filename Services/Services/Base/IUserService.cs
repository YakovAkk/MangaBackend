using Data.Entities;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IUserService
{
    Task<UserEntity> GetUserByNameOrEmail(string nameOrEmail);
    Task<IList<UserEntity>> GetAllAsync();
    Task<bool> UpdateAsync(UserEntity user);
    Task<bool> AddMangaToFavoriteAsync(string userid, string mangaid);
    Task<bool> AddGenreToFavoriteAsync(string userid, string genreid);
    Task<bool> RemoveGenreFromFavoriteAsync(string userid, string genreid);
    Task<bool> RemoveMangaFromFavoriteAsync(string userid, string mangaid);
    Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(string userid);
    Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(string userid);
    Task<UserEntity> GetByIdAsync(string user_id);
    Task<bool> IsUserExists(UserRegistrationDTO userDTO);
}
