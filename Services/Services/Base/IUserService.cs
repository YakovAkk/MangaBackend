using Data.Entities;
using Services.DTO;

namespace Services.Services.Base;

public interface IUserService
{
    Task<UserEntity> CreateAsync(UserDTORegistration user);
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task<UserEntity> LoginAsync(UserDTOLogin userDTOLogin);
    Task<UserEntity> AddMangaToFavoriteAsync(AddTOFavoriteDTO addTOFavoriteDTO);
    Task<UserEntity> AddGenreToFavoriteAsync(AddTOFavoriteDTO addTOFavoriteDTO);
}
