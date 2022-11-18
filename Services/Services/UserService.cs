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
    public async Task<UserEntity> AddGenreToFavoriteAsync(AddTOFavoriteDTO addTOFavoriteDTO)
    {
        if(addTOFavoriteDTO == null)
        {
            var errorMessage = "addTOFavoriteDTO is null";

            throw new ArgumentNullException(errorMessage);
        }

        if(addTOFavoriteDTO.user_Id == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (addTOFavoriteDTO.items_Id == null)
        {
            var errorMessage = "genres_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(addTOFavoriteDTO.user_Id);

            List<GenreEntity> genres = new List<GenreEntity>();

            foreach (var item in addTOFavoriteDTO.items_Id)
            {
                try
                {
                    var genre = await _genreRepository.GetByIdAsync(item);
                    genres.Add(genre);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            var userResult = await _userRespository.AddGenreToFavoriteAsync(user, genres);
            return userResult;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
    public async Task<UserEntity> AddMangaToFavoriteAsync(AddTOFavoriteDTO addTOFavoriteDTO)
    {
        if (addTOFavoriteDTO == null)
        {
            var errorMessage = "addTOFavoriteDTO is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (addTOFavoriteDTO.user_Id == null)
        {
            var errorMessage = "user_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        if (addTOFavoriteDTO.items_Id == null)
        {
            var errorMessage = "mangas_Id is null";

            throw new ArgumentNullException(errorMessage);
        }

        try
        {
            var user = await _userRespository.GetByIdAsync(addTOFavoriteDTO.user_Id);

            List<MangaEntity> mangas = new List<MangaEntity>();

            foreach (var item in addTOFavoriteDTO.items_Id)
            {
                try
                {
                    var manga = await _mangaRepository.GetByIdAsync(item);
                    mangas.Add(manga);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            var userResult = await _userRespository.AddMangaToFavoriteAsync(user, mangas);

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
