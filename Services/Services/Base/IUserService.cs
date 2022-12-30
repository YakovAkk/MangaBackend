using Data.Entities;
using Services.DTO;

namespace Services.Services.Base;

public interface IUserService
{
    Task<IList<UserEntity>> GetAllAsync();
    Task<UserEntity> CreateAsync(UserRegistrationDTO user);
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task<UserEntity> LoginAsync(UserLoginDTO userDTOLogin);
    Task<UserEntity> AddMangaToFavoriteAsync(FavoriteDTO addTOFavoriteDTO);
    Task<UserEntity> AddGenreToFavoriteAsync(FavoriteDTO addTOFavoriteDTO);
    Task<UserEntity> RemoveGenreFromFavoriteAsync(string userid, string genreid);
    Task<UserEntity> RemoveMangaFromFavoriteAsync(string userid, string mangaid);
    Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(string userid);
    Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(string userid);
}
