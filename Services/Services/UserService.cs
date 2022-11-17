using Data.Entities;
using Microsoft.Extensions.Logging;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.ExtensionMapper;
using Services.Services.Base;
using Services.Storage.Base;

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
    public async Task<UserEntity> AddGenreToFavoriteAsync(string user_Id, string genre_Id)
    {
        if(user_Id == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (genre_Id == null)
        {
            var errorMessage = "genre_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(user_Id);
            var genre = await _genreRepository.GetByIdAsync(genre_Id);

            var userResult = await _userRespository.AddGenreToFavoriteAsync(user, genre);

            return userResult;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
    public async Task<UserEntity> AddMangaToFavoriteAsync(string user_Id, string manga_Id)
    {
        if (user_Id == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (manga_Id == null)
        {
            var errorMessage = "genre_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(user_Id);
            var manga = await _mangaRepository.GetByIdAsync(manga_Id);

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
}
