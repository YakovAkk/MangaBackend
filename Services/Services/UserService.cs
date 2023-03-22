using Data.Database;
using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Services.Model.DTO;
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
    public async Task VerifyAsync(UserEntity user)
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
    public async Task<bool> IsUserExists(UserRegistrationDTO userDTO)
    {
        var userByName = await GetUserByNameOrEmail(userDTO.UserName);
        var userByEmail = await GetUserByNameOrEmail(userDTO.Email);

        if(userByName != null)
        {
            throw new Exception("User with the name is alteady registered!");
        }

        if (userByEmail != null)
        {
            throw new Exception("User with the email is alteady registered!");
        }

        return false;
    }
    public async Task<bool> UpdateUserAsync(UserInputModel userInputModel)
    {
        using var dbContext = CreateDbContext();

        var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userInputModel.Id);

        if (existingUser == null)
        {
            var errorMessage = "User doesn't exist";
            throw new Exception(errorMessage);
        }

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
    public async Task<UserEntity> GetByIdAsync(string userId)
    {
        using var dbContext = CreateDbContext();

        var user = await dbContext.Users
            .Include(m => m.FavoriteMangas)
            .Include(m => m.FavoriteGenres)
            .FirstOrDefaultAsync(i => i.Id == userId);

        if (user == null)
        {
            var errorMessage = $"The user doesn't exist!";
            throw new Exception(errorMessage);
        }

        return user;
    }
    public async Task<UserEntity> GetUserByNameOrEmail(string nameOrEmail)
    {
        using var dbContext = CreateDbContext();

        var userExist = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Name == nameOrEmail || x.Email == nameOrEmail);

        if(userExist == null)
        {
            throw new Exception("User doesn't exist!");
        }

        return userExist;
    }

    #endregion

    #region UsersFavorite
    public async Task<bool> AddGenreToFavoriteAsync(string userid, string genreid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        var genre = await _genreService.GetByIdAsync(genreid);

        if (user.FavoriteGenres.Select(x => x.Id).FirstOrDefault(x => x == genre.Id) == null)
            user.FavoriteGenres.Add(genre);
        else
            throw new Exception("User has the genre in favorite already");

        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> AddMangaToFavoriteAsync(string userid, string mangaid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        var genre = await _mangaService.GetByIdAsync(mangaid);

        if (user.FavoriteMangas.Select(x => x.Id).FirstOrDefault(x => x == genre.Id) == null)
            user.FavoriteMangas.Add(genre);
        else
            throw new Exception("User has the manga in favorite already");

        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveGenreFromFavoriteAsync(string userid, string genreid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        var genre = await _genreService.GetByIdAsync(genreid);

        if (user.FavoriteGenres.Select(x => x.Id).FirstOrDefault(x => x == genre.Id) != null)
            user.FavoriteGenres.Remove(genre);
        else
            throw new Exception("User doesn't have the genre in favorite already");

        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveMangaFromFavoriteAsync(string userid, string genreid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        var genre = await _mangaService.GetByIdAsync(genreid);

        if (user.FavoriteMangas.Select(x => x.Id).FirstOrDefault(x => x == genre.Id) != null)
            user.FavoriteMangas.Add(genre);
        else
            throw new Exception("User doesn't have the manga in favorite already");

        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<IList<MangaEntity>> GetAllFavoriteMangaAsync(string userid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        return user.FavoriteMangas;
    }
    public async Task<IList<GenreEntity>> GetAllFavoriteGenreAsync(string userid)
    {
        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(userid);

        return user.FavoriteGenres;
    }
    #endregion
}
