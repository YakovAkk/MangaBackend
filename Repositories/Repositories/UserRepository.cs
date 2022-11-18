using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Repositories.Repositories.Base;

namespace Repositories.Repositories;
public class UserRepository : IUserRespository
{
    private readonly ILogger<GenreRepository> _logger;
    private readonly ITool _logTool;
    private readonly AppDBContent _db;
    public UserRepository(ILogger<GenreRepository> logger, ITool logTool, AppDBContent db)
    {
        _logger = logger;
        _logTool = logTool;
        _db = db;
    }
    public async Task<IList<UserEntity>> GetAllAsync()
    {
        var list = await _db.Users.Include(u => u.FavoriteGenres).Include(u => u.FavoriteMangas).ToListAsync();

        if (list == null)
        {
            return new List<UserEntity>();
        }

        return list;
    }
    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        _logTool.NameOfMethod = nameof(CreateAsync);

        _logTool.WriteToLog(_logger, LogPosition.Begin, $"UserEntity = {user}");

        var userAdded = await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        if(userAdded == null)
        {
            var errorMessage = "User hasn't been added";

            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);  
        }

        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Name == user.Name);

        if(userResult == null)
        {
            var errorMessage = "User hasn't been added";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);
            throw new Exception(errorMessage);
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"userResult = {userResult}");
        return userResult;
    }
    public async Task<UserEntity> GetByIdAsync(string id)
    {
        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

        if(userResult == null) 
        {
            var errorMessage = "User isn't exist";

            throw new ArgumentNullException(errorMessage);
        }

        return userResult;
    }
    public async Task<UserEntity> GetByNameAsync(string name)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u=> u.Name == name);
        if(user == null)
        {
            var errorMessage = "User isn't exist";

            throw new ArgumentNullException(errorMessage);
        }

        return user;
    }
    public async Task<UserEntity> UpdateAsync(UserEntity user)
    {
        var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Name == user.Name);

        if(existingUser == null)
        {
            var errorMessage = "User is null";

            throw new ArgumentNullException(errorMessage);
        }

        var updatedUser = _db.Users.Update(existingUser);

        if(updatedUser == null)
        {
            var errorMessage = "User hasn't been updated";

            throw new Exception(errorMessage);
        }

        await _db.SaveChangesAsync();

        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Name == user.Name);

        if(userResult == null )
        {
            var errorMessage = "User hasn't been updated";

            throw new ArgumentNullException(errorMessage);
        }

        return userResult;
    }
    public async Task<UserEntity> AddGenreToFavoriteAsync(UserEntity user, GenreEntity genres)
    {
        user.FavoriteGenres.Add(genres);

        await _db.SaveChangesAsync();

        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        if(userResult == null)
        {
            var errorMessage = "User hasn't been updated";

            throw new ArgumentNullException(errorMessage);
        }

        return userResult;
    }
    public async Task<UserEntity> AddMangaToFavoriteAsync(UserEntity user, MangaEntity mangas)
    {
        user.FavoriteMangas.Add(mangas);

        await _db.SaveChangesAsync();

        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userResult == null)
        {
            var errorMessage = "User hasn't been updated";

            throw new ArgumentNullException(errorMessage);
        }

        return userResult;
    }
    public async Task<UserEntity> RemoveGenreFromFavoriteAsync(UserEntity user, GenreEntity manga)
    {
        user.FavoriteGenres.Remove(manga);

        await _db.SaveChangesAsync();

        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userResult == null)
        {
            var errorMessage = "User hasn't been updated";

            throw new ArgumentNullException(errorMessage);
        }

        return userResult;
    }
    public async Task<UserEntity> RemoveMangaFromFavoriteAsync(UserEntity user, MangaEntity manga)
    {
        user.FavoriteMangas.Remove(manga);

        await _db.SaveChangesAsync();

        var userResult = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userResult == null)
        {
            var errorMessage = "User hasn't been updated";

            throw new ArgumentNullException(errorMessage);
        }

        return userResult;
    }
}
