using Data.Entities;
using Services.Model.DTO;

namespace Services.Services.Base;

public interface IUserService
{
    Task<UserEntity> GetUserByNameOrEmail(string nameOrEmail);
    Task<IList<UserEntity>> GetAllAsync();
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task<UserEntity> AddMangaToFavoriteAsync(FavoriteDTO addTOFavoriteDTO);
    Task<UserEntity> AddGenreToFavoriteAsync(FavoriteDTO addTOFavoriteDTO);
    Task<UserEntity> RemoveGenreFromFavoriteAsync(string userid, string genreid);
    Task<UserEntity> RemoveMangaFromFavoriteAsync(string userid, string mangaid);
    Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(string userid);
    Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(string userid);
    Task<UserEntity> GetByIdAsync(string user_id);
}
