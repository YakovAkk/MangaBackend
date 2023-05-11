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
    public async Task<bool> IsUserExistsAsync(string email, string name)
    {
        var userByName = await GetUserByNameAsync(name);
        var userByEmail = await GetUserByEmailAsync(email);

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
    public async Task<List<UserEntity>> GetAllAsync()
    {
        using var dbContext = CreateDbContext();

        var list = await dbContext.Users.ToListAsync();

        return list;
    }
    public async Task<UserEntity> GetByIdAsync(int userId)
    {
        using var dbContext = CreateDbContext();

        var user = await dbContext.Users
            .Include(x => x.FavoriteGenresItems)
                .ThenInclude(x => x.Genre)
            .Include(x => x.FavoriteMangasItems)
                .ThenInclude(x => x.Manga)
            .FirstOrDefaultAsync(i => i.Id == userId);

        if (user == null)
            throw new Exception("The user doesn't exist!");
        
        return user;
    }
    public async Task<UserEntity> GetUserByNameAsync(string name)
    {
        using var dbContext = CreateDbContext();

        return await dbContext.Users.FirstOrDefaultAsync(x => x.Name == name);
    }
    public async Task<UserEntity> GetUserByEmailAsync(string email)
    {
        using var dbContext = CreateDbContext();

        return await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    #endregion

    #region UsersFavorite
    public async Task<bool> AddGenreToFavoriteAsync(int userId, int genreId)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userId);
        var genre = await _genreService.GetByIdAsync(genreId);

        if (!user.FavoriteGenres.Select(x => x.Id).Contains(genreId))
            user.FavoriteGenresItems.Add(new FavoriteGenreEntity(user, genre));
        else
            throw new Exception("User has the genre in favorite already");

        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> AddMangaToFavoriteAsync(int userId, int mangaId)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userId);
        var manga = await _mangaService.GetByIdAsync(mangaId);

        if (!user.FavoriteMangas.Select(x => x.Id).Contains(mangaId))
            user.FavoriteMangasItems.Add(new FavoriteMangaEntity(user, manga));
        else
            throw new Exception("User has the manga in favorite already");

        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveGenreFromFavoriteAsync(int userId, int genreId)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userId);
        var genre = await _genreService.GetByIdAsync(genreId);

        if (user.FavoriteGenres.Select(x => x.Id).Contains(genreId))
            user.FavoriteGenresItems.Remove(user.FavoriteGenresItems.Where(x => x.Genre.Id == genreId).Single());
        else
            throw new Exception("User doesn't have the genre in favorite already");

        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveMangaFromFavoriteAsync(int userId, int mangaId)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userId);
        var manga = await _mangaService.GetByIdAsync(mangaId);

        if (user.FavoriteMangas.Select(x => x.Id).Contains(mangaId))
            user.FavoriteMangasItems.Add(user.FavoriteMangasItems.Where(x => x.Manga.Id == mangaId).Single());
        else
            throw new Exception("User doesn't have the manga in favorite already");

        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<List<MangaEntity>> GetAllFavoriteMangasAsync(int userId)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userId);

        return user.FavoriteMangas;
    }
    public async Task<List<GenreEntity>> GetAllFavoriteGenresAsync(int userId)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userId);

        return user.FavoriteGenres;
    }
    #endregion
}
