using Data.Database;
using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Services.Model.InputModel;
using Services.Services.Base;

namespace Services.Services;

public class UserService : DbService<AppDBContext>, IUserService
{
    private readonly IGenreService _genreService;
    private readonly IMangaService _mangaService;

    public UserService(IGenreService genreService, IMangaService mangaService,
        DbContextOptions<AppDBContext> dbContextOptions) : base(dbContextOptions)
    {
        _genreService = genreService;
        _mangaService = mangaService;
    }

    #region Auth
    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        using var dbContext = CreateDbContext();

        await dbContext.Users.AddAsync(user);

        await dbContext.SaveChangesAsync();

        return user;
    }
    public async Task SetRefreshToken(RefreshToken refreshToken, UserEntity user)
    {
        using var dbContext = CreateDbContext();

        user.RefreshToken = refreshToken.Token;
        user.TokenExpires = refreshToken.Expires;
        user.TokenCreated = refreshToken.Created;

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }
    public async Task SetVerivicationAsync(UserEntity user)
    {
        using var dbContext = CreateDbContext();
        user.VerifiedAt = DateTime.UtcNow;
        dbContext.Users.Update(user);

        await dbContext.SaveChangesAsync();
    }
    public async Task SetResetPasswordToken(ResetPasswordToken resetPasswordToken, UserEntity user)
    {
        using var dbContext = CreateDbContext();

        user.ResetPasswordToken = resetPasswordToken.Token;
        user.ResetPasswordTokenExpires = resetPasswordToken.Expires;

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }
    #endregion

    #region User
    public async Task<bool> IsUserExists(string email, string name)
    {
        var userByName = await GetUserByNameOrEmail(name);
        var userByEmail = await GetUserByNameOrEmail(email);

        return userByName != null || userByEmail != null;
    }
    public async Task<bool> UpdateUserAsync(UserInputModel userInputModel)
    {
        using var dbContext = CreateDbContext();

        var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userInputModel.Id);

        if (existingUser == null)
            throw new Exception("User doesn't exist");
        
        if(!string.IsNullOrEmpty(userInputModel.Name))
            existingUser.Name = userInputModel.Name;

        dbContext.Users.Update(existingUser);

        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<IList<UserEntity>> GetAllAsync()
    {
        using var dbContext = CreateDbContext();

        var list = await dbContext.Users.ToListAsync();

        return list;
    }
    public async Task<UserEntity> GetByIdAsync(int userId)
    {
        using var dbContext = CreateDbContext();

        var user = await dbContext.Users
            .Include(m => m.FavoriteMangas)
            .Include(m => m.FavoriteGenres)
            .FirstOrDefaultAsync(i => i.Id == userId);

        if (user == null)
            throw new Exception("The user doesn't exist!");
        
        return user;
    }
    public async Task<UserEntity> GetUserByNameOrEmail(string nameOrEmail)
    {
        using var dbContext = CreateDbContext();

        return await dbContext.Users
            .FirstOrDefaultAsync(x => x.Name == nameOrEmail || x.Email == nameOrEmail);
    }

    #endregion

    #region UsersFavorite
    public async Task<bool> AddGenreToFavoriteAsync(int userid, int genreid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        var genre = await _genreService.GetByIdAsync(genreid);

        if (await _genreService.IsGenreExistAsync(genreid))
            user.FavoriteGenres.Add(genre);
        else
            throw new Exception("User has the genre in favorite already");

        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> AddMangaToFavoriteAsync(int userid, int mangaId)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        var manga = await _mangaService.GetByIdAsync(mangaId);

        if (await _mangaService.IsMangaExistAsync(mangaId))
            user.FavoriteMangas.Add(manga);
        else
            throw new Exception("User has the manga in favorite already");

        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveGenreFromFavoriteAsync(int userid, int genreid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        var genre = await _genreService.GetByIdAsync(genreid);

        if (await _genreService.IsGenreExistAsync(genreid))
            user.FavoriteGenres.Remove(genre);
        else
            throw new Exception("User doesn't have the genre in favorite already");

        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveMangaFromFavoriteAsync(int userid, int mangaId)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        var manga = await _mangaService.GetByIdAsync(mangaId);

        if (await _mangaService.IsMangaExistAsync(mangaId))
            user.FavoriteMangas.Add(manga);
        else
            throw new Exception("User doesn't have the manga in favorite already");

        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(int userid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        return user.FavoriteMangas;
    }
    public async Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(int userid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        return user.FavoriteGenres;
    }
    #endregion
}
