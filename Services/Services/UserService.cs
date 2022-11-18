using Data.Entities;
using Microsoft.Extensions.Logging;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.ExtensionMapper;
using Services.Services.Base;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Services.Services;

public class UserService : IUserService
{
    private readonly IUserRespository _userRespository;
    private readonly IGenreRepository _genreRepository;
    private readonly IMangaRepository _mangaRepository;
    private readonly ILogger<UserService> _logger;
    private readonly ITool _logTool;

    public UserService(IUserRespository userRespository, IGenreRepository genreRepository, IMangaRepository mangaRepository, ILogger<UserService> logger, ITool logTool)
    {
        _userRespository = userRespository;
        _genreRepository = genreRepository;
        _mangaRepository = mangaRepository;
        _logger = logger;
        _logTool = logTool;
    }

    public async Task<UserEntity> CreateAsync(UserDTORegistration userDTO)
    {
        _logTool.NameOfMethod = nameof(CreateAsync);

        _logTool.WriteToLog(_logger, LogPosition.Begin, $"UserDTORegistration = {userDTO}");

        if (userDTO == null)
        {
            var errorMessage = "User is null";

            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new ArgumentNullException(errorMessage);
        }

        var userModel = userDTO.toEntity();

        try
        {
            var res = await _userRespository.CreateAsync(userModel);

            _logTool.WriteToLog(_logger, LogPosition.End, $"user = {res}");

            return res;
        }
        catch (Exception ex)
        {
            _logTool.WriteToLog(_logger, LogPosition.Error, ex.Message);
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
    public async Task<UserEntity> AddGenreToFavoriteAsync(FavoriteDTO addTOFavoriteDTO)
    {
        if(addTOFavoriteDTO == null)
        {
            var errorMessage = "addTOFavoriteDTO is null";

            throw new ArgumentNullException(errorMessage);
        }

        if(addTOFavoriteDTO.User_Id == null)
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
    public async Task<UserEntity> LoginAsync(UserDTOLogin userDTOLogin)
    {
        if(userDTOLogin == null)
        {
            var errorMessage = "User is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var userName = userDTOLogin.Name;

            if(userName == null)
            {
                var errorMessage = "userName is null";

                throw new ArgumentNullException(errorMessage);
            }

            var userExist = await _userRespository.GetByNameAsync(userDTOLogin.Name);

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
}
