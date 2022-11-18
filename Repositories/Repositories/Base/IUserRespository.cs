using Data.Entities;

namespace Repositories.Repositories.Base;
public interface IUserRespository
{
    Task<UserEntity> CreateAsync(UserEntity user);
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task<UserEntity> GetByIdAsync(string id);
    Task<UserEntity> GetByNameAsync(string name);
    Task<UserEntity> AddMangaToFavoriteAsync(UserEntity user, List<MangaEntity> manga);
    Task<UserEntity> AddGenreToFavoriteAsync(UserEntity user, List<GenreEntity> genre);
}
