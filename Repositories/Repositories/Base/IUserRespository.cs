using Data.Entities;

namespace Repositories.Repositories.Base;
public interface IUserRespository
{
    Task<IList<UserEntity>> GetAllAsync();
    Task<UserEntity> CreateAsync(UserEntity user);
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task<UserEntity> GetByIdAsync(string id);
    Task<UserEntity> GetByNameAsync(string name);
    Task<UserEntity> AddMangaToFavoriteAsync(UserEntity user, MangaEntity manga);
    Task<UserEntity> AddGenreToFavoriteAsync(UserEntity user, GenreEntity genre);
    Task<UserEntity> RemoveGenreFromFavoriteAsync(UserEntity user, GenreEntity manga);
    Task<UserEntity> RemoveMangaFromFavoriteAsync(UserEntity user, MangaEntity manga);
    Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(UserEntity user);
    Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(UserEntity user);
}
