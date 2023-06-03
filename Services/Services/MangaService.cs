using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Core;
using Services.Core.Paginated;
using Services.Extensions.ExtensionMapper;
using Services.Model.Configuration;
using Services.Model.DTO;
using Services.Model.ViewModel;
using Services.Services.Base;
using ValidateService.Validate;

namespace Services.Services;

public class MangaService : DbService<AppDBContext>, IMangaService
{
    private readonly OthersConfiguration _othesConfiguration;

    public MangaService(
        OthersConfiguration othesConfiguration,
        DbContextOptions<AppDBContext> dbContextOptions) : base(dbContextOptions)
    {
        _othesConfiguration = othesConfiguration;
    }

    public async Task<List<MangaViewModel>> GetAllAsync()
    {
        List<MangaEntity> mangas;

        using( var dbContext = CreateDbContext())
        {
            mangas = await dbContext.Mangas
            .Include(m => m.Genres)
            .Include(m => m.PathToFoldersWithGlava)
            .OrderBy(m => m.Name)
            .ToListAsync();
        }

        foreach (var genre in mangas.SelectMany(x => x.Genres))
            genre.CleanMangas();
        
        foreach (var item in mangas)
        {
            item.PathToTitlePicture = $"{_othesConfiguration.RelativePath}{item.PathToTitlePicture}";
            foreach (var res in item.PathToFoldersWithGlava)
            {
                res.LinkToFirstPicture = $"{_othesConfiguration.RelativePath}{res.LinkToFirstPicture}";
            }
        }

        var viewModels = mangas.Select(x => x.MapTo<MangaViewModel>()).ToList();

        for (int i = 0; i < mangas.Count; i++)
        {
            viewModels[i].PathToFoldersWithGlava = mangas[i].PathToFoldersWithGlava.Select(x => x.MapTo<GlavaMangaEntityViewModel>()).ToList();
        }

        return viewModels;
    }
    public async Task<List<MangaViewModel>> AddRangeAsync(List<MangaInput> mangas)
    {
        using var dbContext = CreateDbContext();

        var genresIds = mangas.SelectMany(x => x.Genres_Ids);

        var genres = dbContext.Genres.Where(x => genresIds.Contains(x.Id)).ToList();

        foreach (var manga in mangas)
        {
            var genresForManga = genres.Where(x => manga.Genres_Ids.Contains(x.Id)).ToList();

            var mangaEntity = manga.toEntity(genresForManga);

            if (dbContext.Mangas.SingleOrDefault(x => x.Name == mangaEntity.Name) == null)
                dbContext.Mangas.Add(mangaEntity);
        }

        await dbContext.SaveChangesAsync();

        return await GetAllAsync();
    }
    public async Task<MangaEntity> GetByIdAsync(int Id)
    {
        using var dbContext = CreateDbContext();

        var manga = await dbContext.Mangas
            .Include(m => m.Genres)
            .Include(m => m.PathToFoldersWithGlava)
            .FirstOrDefaultAsync(i => i.Id == Id);

        if (manga == null)
            throw new Exception($"The manga isn't contained in the database!");
        

        foreach (var genre in manga.Genres)
            genre.CleanMangas();

        manga.PathToTitlePicture = $"{_othesConfiguration.RelativePath}{manga.PathToTitlePicture}";

        foreach (var res in manga.PathToFoldersWithGlava)
        {
            res.LinkToFirstPicture = $"{_othesConfiguration.RelativePath}{res.LinkToFirstPicture}";
        }

        return manga;
    }
    public async Task<PagedResult<List<MangaEntity>, object>> GetPagiantedMangaListAsync(int sizeOfPage, int page)
    {
        if (!ValidatorService.IsValidPageAndPageSize(sizeOfPage, page))
            throw new Exception("Parameters aren't valid");
        

        IQueryable<MangaEntity> Query(AppDBContext dbContext)
        {
            return dbContext.Mangas;
        }

        int count;
        List<MangaEntity> mangaResult;

        using (var contextPool = new ContextPool<AppDBContext>(() => CreateDbContext()))
        {
            var dataTask = Query(contextPool.NewContext())
                  .Include(m => m.Genres)
                  .Include(m => m.PathToFoldersWithGlava)
                  .Skip((page - 1) * sizeOfPage)
                  .Take(sizeOfPage)
                  .ToListAsync();

            var countTask = Query(contextPool.NewContext()).CountAsync();
            await Task.WhenAll(dataTask, countTask);

            mangaResult = dataTask.Result;
            count = countTask.Result;
        }


        foreach (var genre in mangaResult.SelectMany(x => x.Genres))
        {
            genre.CleanMangas();
        }

        return new PagedResult<List<MangaEntity>, object>(count, mangaResult, null);
    }
    public async Task<List<MangaEntity>> FiltrationByDateAsync(int year)
    {
        int yearNum = 0;

        if (!ValidatorService.IsValidYear(year))
            throw new Exception("Parameters aren't valid");

        using var dbContext = CreateDbContext();

        var list = await dbContext.Mangas
            .Include(m => m.Genres)
            .Include(i => i.PathToFoldersWithGlava)
            .Where(i => i.ReleaseYear > yearNum)
            .ToListAsync();

        foreach (var genre in list.SelectMany(x => x.Genres))
        {
            genre.CleanMangas();
        }

        return list;
    }
    public async Task<List<MangaEntity>> FiltrationByNameAsync(string name)
    {
        using var dbContext = CreateDbContext();

        var list = await dbContext.Mangas
            .Include(m => m.Genres)
            .Include(i => i.PathToFoldersWithGlava)
            .Where(i => i.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();

        foreach (var genre in list.SelectMany(x => x.Genres))
        {
            genre.CleanMangas();
        }

        return list;
    }
    public async Task<bool> IsMangaExistAsync(int mangaId)
    {
        using var dbContext = CreateDbContext();

        var manga = await dbContext.Mangas.FirstOrDefaultAsync(x => x.Id == mangaId);

        return manga != null;
    }
}
