using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Core;
using Services.Model.Helping;
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
    public async Task<List<UserViewModel>> GetAllAsync()
    {
        using var dbContext = CreateDbContext();

        var list = await dbContext.Users.ToListAsync();

        return list.Select(x => x.MapTo<UserViewModel>()).ToList();
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
            .Include(x => x.Roles)
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
                var userViewModel = user.MapTo<UserViewModel>();

                var viewModel = item.MapTo<RememberReadingItemViewModel>();
                viewModel.User = userViewModel;
                viewModel.Manga = manga;

                result.Add(viewModel);
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

            var readingModel = inputModel.MapTo<RememberReadingItem>();
            readingModel.User = user;
            readingModel.MangaId = manga.Id;

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
            var userViewModel = user.MapTo<UserViewModel>();

            var viewModel = item.MapTo<RememberReadingItemViewModel>();
            viewModel.User = userViewModel;
            viewModel.Manga = manga;

            return viewModel;
        }
    }
    #endregion

    #region Recommendations
    public async Task<List<MangaViewModel>> GetRecommendationsAsync(string id)
    {
        var userId = Convert.ToInt32(id);
        var user = await GetByIdAsync(userId);

        var sharedMangasIds = new List<int>();
        var favoriteMangasIds = user.FavoriteMangasItems.Select(x => x.Id).ToList();

        foreach (var item in user.RememberReadingItems)
            if(favoriteMangasIds.Contains(item.MangaId))
                sharedMangasIds.Add(item.MangaId);

        List<int> allMangasWithoutSharedGenresIds;
        List<int> sharedGenresIds;
        List<GenreEntity> recomendedGenres;
        using (var contextPool = new ContextPool<AppDBContext>(() => CreateDbContext()))
        {
            var allMangas = _mangaService.GetAllInternalAsync(contextPool.NewContext());

            var allMangasWithoutSharedGenresIdsTask = allMangas
                .Where(x => !sharedMangasIds.Contains(x.Id))
                .SelectMany(x => x.Genres)
                .Select(x => x.Id)
                .ToListAsync();

            var sharedGenresIdsTask = _mangaService
                .GetRangeByIdInternalAsync(sharedMangasIds, contextPool.NewContext())
                .SelectMany(x => x.Genres)
                .Select(x => x.Id)
                .ToListAsync();

            await Task.WhenAll(allMangasWithoutSharedGenresIdsTask, sharedGenresIdsTask);

            allMangasWithoutSharedGenresIds = allMangasWithoutSharedGenresIdsTask.Result;
            sharedGenresIds = sharedGenresIdsTask.Result;

            var recomendedGenresIds = new List<int>();

            foreach (var item in allMangasWithoutSharedGenresIds)
                if (sharedGenresIds.Contains(item))
                    recomendedGenresIds.Add(item);

            recomendedGenres = await _genreService.GetRangeByIdInternalAsync(recomendedGenresIds, contextPool.NewContext()).ToListAsync();
        }
        
        var recomendedMangas = recomendedGenres
            .SelectMany(x => x.Mangas)
            .GroupBy(x => x.Id)
            .Select(g => g.First())
            .ToList();

        var result = recomendedMangas.Select(x => x.MapTo<MangaViewModel>()).ToList();
        return result;
    }
    #endregion

}
