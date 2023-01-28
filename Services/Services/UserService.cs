using Data.Entities;
using Microsoft.Extensions.Logging;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.ExtensionMapper;
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

    #region UserAuthorization
    public async Task<UserEntity> CreateAsync(UserRegistrationDTO userDTO)
    {

        if (userDTO == null)
        {
            var errorMessage = "User is null";
            throw new ArgumentNullException(errorMessage);
        }

        if(userDTO.Password != userDTO.ConfirmPassword)
        {
            var errorMessage = "Both of passwords must be equal!";
            throw new Exception(errorMessage);
        }

        var userModel = userDTO.toEntity();

        try
        {
            var res = await _userRespository.CreateAsync(userModel);
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async Task<UserEntity> UpdateAsync(UserEntity user)
    {
        if(user == null)
        {
            var errorMessage = "User is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var updatedUser = await _userRespository.UpdateAsync(user);

            return updatedUser;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<UserEntity> LoginAsync(UserLoginDTO userDTOLogin)
    {
        if(userDTOLogin == null)
        {
            var errorMessage = "User is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (string.IsNullOrEmpty(userDTOLogin.NameOrEmail))
        {
            var errorMessage = "Name Or Email field is null or empty";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var userExist = await GetUserByNameOrEmail(userDTOLogin.NameOrEmail);

            return userExist;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }  

    }
    public async Task<IList<UserEntity>> GetAllAsync()
    {
        return await _userRespository.GetAllAsync();
    }
    #endregion

    #region UsersFavorite
    
    public async Task<UserEntity> AddGenreToFavoriteAsync(FavoriteDTO addTOFavoriteDTO)
    {
        if (addTOFavoriteDTO == null)
        {
            var errorMessage = "addTOFavoriteDTO is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (addTOFavoriteDTO.User_Id == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (addTOFavoriteDTO.Item_Id == null)
        {
            var errorMessage = "genres_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(addTOFavoriteDTO.User_Id);

            var genre = await _genreRepository.GetByIdAsync(addTOFavoriteDTO.Item_Id);

            var userResult = await _userRespository.AddGenreToFavoriteAsync(user, genre);
            return userResult;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
    public async Task<UserEntity> AddMangaToFavoriteAsync(FavoriteDTO addTOFavoriteDTO)
    {
        if (addTOFavoriteDTO == null)
        {
            var errorMessage = "addTOFavoriteDTO is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (addTOFavoriteDTO.User_Id == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (addTOFavoriteDTO.Item_Id == null)
        {
            var errorMessage = "mangas_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(addTOFavoriteDTO.User_Id);

            var manga = await _mangaRepository.GetByIdAsync(addTOFavoriteDTO.Item_Id);

            var userResult = await _userRespository.AddMangaToFavoriteAsync(user, manga);

            return userResult;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<UserEntity> RemoveGenreFromFavoriteAsync(string userid, string genreid)
    {
        if (userid == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (genreid == null)
        {
            var errorMessage = "manga_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(userid);

            var genre = await _genreRepository.GetByIdAsync(genreid);

            var userResult = await _userRespository.RemoveGenreFromFavoriteAsync(user, genre);

            return userResult;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
    public async Task<UserEntity> RemoveMangaFromFavoriteAsync(string userid, string genreid)
    {
        if (userid == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (genreid == null)
        {
            var errorMessage = "manga_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(userid);

            var manga = await _mangaRepository.GetByIdAsync(genreid);

            var userResult = await _userRespository.RemoveMangaFromFavoriteAsync(user, manga);

            return userResult;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
    public async Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(string userid)
    {
        if (userid == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(userid);

            var list = await _userRespository.GetAllFavoriteMangaAsync(user);

            return list;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }  
    }
    public async Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(string userid)
    {

        if (userid == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(userid);

            var list = await _userRespository.GetAllFavoriteGenreAsync(user);

            return list;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion

    #region Private
    private async Task<UserEntity> GetUserByNameOrEmail(string nameOrEmail)
    {
        UserEntity userExistByName = null;
        UserEntity userExistByEmail = null;

        try
        {
            userExistByName = await _userRespository.GetByNameAsync(nameOrEmail);
        }
        catch (Exception)
        {
            
        }
        try
        {
            userExistByEmail = await _userRespository.GetByEmailAsync(nameOrEmail);
        }
        catch (Exception)
        {

           
        }

        if(userExistByName == null && userExistByEmail == null)
        {
            var errorMessage = "User doesn't exist";

            throw new ArgumentNullException(errorMessage);
        }

        return userExistByName ?? userExistByEmail;
    }

    #endregion
}
