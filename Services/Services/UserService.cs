using Data.Entities;
using Microsoft.Extensions.Logging;
using Repositories.Repositories.Base;
using Services.Model.DTO;
using Services.Services.Base;

namespace Services.Services;

public class UserService : IUserService
{
    private readonly IUserRespository _userRespository;
    private readonly IGenreRepository _genreRepository;
    private readonly IMangaRepository _mangaRepository;
    private readonly ILogger<UserService> _logger;
    public UserService(IUserRespository userRespository, IGenreRepository genreRepository, IMangaRepository mangaRepository)
    {
        _userRespository = userRespository;
        _genreRepository = genreRepository;
        _mangaRepository = mangaRepository;
    }

    #region User
    public async Task<bool> IsUserExists(UserRegistrationDTO userDTO)
    {
        try
        {
            var userName = await GetUserByNameOrEmail(userDTO.UserName);

            var userEmail = await GetUserByNameOrEmail(userDTO.Email);

            if(userName != null && userEmail != null)
            {
                return true;
            }
        }
        catch (Exception)
        {
 
        }

        return false;

    }

    public async Task<bool> UpdateAsync(UserEntity user)
    {
        var updatedUser = await _userRespository.UpdateAsync(user);

        if(updatedUser != null)
        {
            return true;
        }
        
        return false;
    }
    public async Task<IList<UserEntity>> GetAllAsync()
    {
        return await _userRespository.GetAllAsync();
    }

    public async Task<UserEntity> GetByIdAsync(string user_id)
    {
        return await _userRespository.GetByIdAsync(user_id);
    }

    public async Task<UserEntity> GetUserByNameOrEmail(string nameOrEmail)
    {
        UserEntity userExistByName = null;
        UserEntity userExistByEmail = null;

        userExistByName = await _userRespository.GetByNameAsync(nameOrEmail);

        userExistByEmail = await _userRespository.GetByEmailAsync(nameOrEmail);

        if (userExistByName == null && userExistByEmail == null)
        {
            var errorMessage = "User doesn't exist";

            throw new Exception(errorMessage);
        }

        return userExistByName ?? userExistByEmail;
    }

    #endregion

    #region UsersFavorite
    public async Task<bool> AddGenreToFavoriteAsync(FavoriteDTO addTOFavoriteDTO)
    {
        var user = await _userRespository.GetByIdAsync(addTOFavoriteDTO.User_Id);

        var genre = await _genreRepository.GetByIdAsync(addTOFavoriteDTO.Item_Id);

        var userResult = await _userRespository.AddGenreToFavoriteAsync(user, genre);

        if(userResult != null)
        {
            return true;
        }

        return false;
    }
    public async Task<bool> AddMangaToFavoriteAsync(FavoriteDTO addTOFavoriteDTO)
    {
        var user = await _userRespository.GetByIdAsync(addTOFavoriteDTO.User_Id);

        var manga = await _mangaRepository.GetByIdAsync(addTOFavoriteDTO.Item_Id);

        var userResult = await _userRespository.AddMangaToFavoriteAsync(user, manga);

        if (userResult != null)
        {
            return true;
        }

        return false;
    }
    public async Task<bool> RemoveGenreFromFavoriteAsync(string userid, string genreid)
    {
        var user = await _userRespository.GetByIdAsync(userid);

        var genre = await _genreRepository.GetByIdAsync(genreid);

        var userResult = await _userRespository.RemoveGenreFromFavoriteAsync(user, genre);

        if (userResult != null)
        {
            return true;
        }

        return false;
    }
    public async Task<bool> RemoveMangaFromFavoriteAsync(string userid, string genreid)
    {
        var user = await _userRespository.GetByIdAsync(userid);

        var manga = await _mangaRepository.GetByIdAsync(genreid);

        var userResult = await _userRespository.RemoveMangaFromFavoriteAsync(user, manga);

        if (userResult != null)
        {
            return true;
        }

        return false;
    }
    public async Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(string userid)
    {
        var user = await _userRespository.GetByIdAsync(userid);

        var list = await _userRespository.GetAllFavoriteMangaAsync(user);

        return list;
    }
    public async Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(string userid)
    {
        var user = await _userRespository.GetByIdAsync(userid);

        var list = await _userRespository.GetAllFavoriteGenreAsync(user);

        return list;
    }


    #endregion
}
