using Data.Entities;
using Services.DTO;

namespace Services.Services.Base;

public interface IUserService
{
    Task<IList<UserEntity>> GetAllAsync();
    Task<UserEntity> CreateAsync(UserDTORegistration user);
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task<UserEntity> LoginAsync(UserDTOLogin userDTOLogin);
    Task<UserEntity> AddMangaToFavoriteAsync(FavoriteDTO addTOFavoriteDTO);
    Task<UserEntity> AddGenreToFavoriteAsync(FavoriteDTO addTOFavoriteDTO);
    Task<UserEntity> RemoveGenreFromFavoriteAsync(FavoriteDTO addTOFavoriteDTO);
    Task<UserEntity> RemoveMangaFromFavoriteAsync(FavoriteDTO addTOFavoriteDTO);
}
