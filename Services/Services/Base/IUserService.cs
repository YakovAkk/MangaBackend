using Data.Entities;
using Services.DTO;

namespace Services.Services.Base;

public interface IUserService
{
    Task<UserEntity> CreateAsync(UserDTORegistration user);
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task<UserEntity> LoginAsync(UserDTOLogin userDTOLogin);
    Task<UserEntity> AddMangaToFavoriteAsync(string user_Id, string manga_Id);
    Task<UserEntity> AddGenreToFavoriteAsync(string user_Id, string genre_Id);
}
