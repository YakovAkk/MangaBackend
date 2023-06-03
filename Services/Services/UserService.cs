using Data.Database;
using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Services.Extensions.ExtensionMapper;
using Services.Model.InputModel;
using Services.Model.ViewModel;
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
            .Include(x => x.RememberReadingItems)
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
    public async Task<bool> AddGenreToFavoriteAsync(string userId, int genreId)
    {
        var Id = Convert.ToInt32(userId);

        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(Id);
        var genre = await _genreService.GetByIdAsync(genreId);

        if (!user.FavoriteGenres.Select(x => x.Id).Contains(genreId))
            user.FavoriteGenresItems.Add(new FavoriteGenreEntity(user, genre));
        else
            throw new Exception("User has the genre in favorite already");

        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> AddMangaToFavoriteAsync(string userId, int mangaId)
    {
        var Id = Convert.ToInt32(userId);

        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(Id);
        var manga = await _mangaService.GetByIdAsync(mangaId);

        if (!user.FavoriteMangas.Select(x => x.Id).Contains(mangaId))
            user.FavoriteMangasItems.Add(new FavoriteMangaEntity(user, manga));
        else
            throw new Exception("User has the manga in favorite already");

        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveGenreFromFavoriteAsync(string userId, int genreId)
    {
        var Id = Convert.ToInt32(userId);

        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(Id);
        var genre = await _genreService.GetByIdAsync(genreId);

        if (user.FavoriteGenres.Select(x => x.Id).Contains(genreId))
            user.FavoriteGenresItems.Remove(user.FavoriteGenresItems.Where(x => x.Genre.Id == genreId).Single());
        else
            throw new Exception("User doesn't have the genre in favorite already");

        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveMangaFromFavoriteAsync(string userId, int mangaId)
    {
        var Id = Convert.ToInt32(userId);

        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(Id);
        var manga = await _mangaService.GetByIdAsync(mangaId);

        if (user.FavoriteMangas.Select(x => x.Id).Contains(mangaId))
            user.FavoriteMangasItems.Add(user.FavoriteMangasItems.Where(x => x.Manga.Id == mangaId).Single());
        else
            throw new Exception("User doesn't have the manga in favorite already");

        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<List<MangaEntity>> GetAllFavoriteMangasAsync(string userId)
    {
        var Id = Convert.ToInt32(userId);

        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(Id);

        return user.FavoriteMangas;
    }
    public async Task<List<GenreEntity>> GetAllFavoriteGenresAsync(string userId)
    {
        var Id = Convert.ToInt32(userId);

        using var dbContext = CreateDbContext();

        var user = await GetByIdAsync(Id);

        return user.FavoriteGenres;
    }
    #endregion

    #region RememberReaading

    public async Task<List<RememberReadingItemViewModel>> GetAllReadingItemsAsync(string userId)
    {
        var id = Convert.ToInt32(userId);

        using (var dbContext = CreateDbContext())
        {
            var user = await GetByIdAsync(id);
            if (user == null)
                throw new Exception("User doesn't exist!");

            var result = new List<RememberReadingItemViewModel>();

            foreach (var item in user.RememberReadingItems)
            {
                var manga = await _mangaService.GetByIdAsync(item.MangaId);
                var userViewModel = user.toViewModel();

                result.Add(item.toViewModel(userViewModel,manga));
            }

            return result;
        }
    }

    public async Task CreateReadingItemAsync(string userId, RememberReadingItemInputModel inputModel)
    {
        var id = Convert.ToInt32(userId);

        using (var dbContext = CreateDbContext())
        {
            var user = await GetByIdAsync(id);
            if (user == null)
                throw new Exception("User doesn't exist!");

            var manga = await _mangaService.GetByIdAsync(inputModel.MangaId);
            var readingModel = inputModel.toEntity(user, manga);

            user.RememberReadingItems.Add(readingModel);

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<RememberReadingItemViewModel> GetReadingItemAsync(string userId, string mangaId)
    {
        var id = Convert.ToInt32(userId);
        var mId = Convert.ToInt32(mangaId);
        using (var dbContext = CreateDbContext())
        {
            var user = await GetByIdAsync(id);
            if (user == null)
                throw new Exception("User doesn't exist!");

            var result = new RememberReadingItemViewModel();

            var item = user.RememberReadingItems.FirstOrDefault(x => x.MangaId == mId);
            if(item == null)
                throw new Exception("User hasn't read the manga yet!");

            var manga = await _mangaService.GetByIdAsync(item.MangaId);
            var userViewModel = user.toViewModel();

            return item.toViewModel(userViewModel, manga);
        }
    }
    #endregion

}
