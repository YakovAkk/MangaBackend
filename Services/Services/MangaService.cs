using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Core;
using Services.Core.Paginated;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Services.Base;
using Services.Storage.Base;
using System;
using System.Collections.Generic;
using ValidateService.Validate;

namespace Services.Services;

public class MangaService : DbService<AppDBContext>, IMangaService
{
    private readonly ILocalStorage _localStorage;

    public MangaService(
        ILocalStorage localStorage,
        DbContextOptions<AppDBContext> dbContextOptions) : base(dbContextOptions)
    {
        _localStorage = localStorage;
    }

    public async Task<IList<MangaEntity>> GetAllAsync()
    {
        using var dbContext = CreateDbContext();

        var list = await dbContext.Mangas
            .Include(m => m.Genres)
            .Include(m => m.PathToFoldersWithGlava)
            .ToListAsync();

        foreach (var genre in list.SelectMany(x => x.Genres))
            genre.CleanMangas();
        

        foreach (var item in list)
        {
            item.PathToTitlePicture = $"{_localStorage.RelativePath}{item.PathToTitlePicture}";
            foreach (var res in item.PathToFoldersWithGlava)
            {
                res.LinkToFirstPicture = $"{_localStorage.RelativePath}{res.LinkToFirstPicture}";
            }
        }

        return list;
    }
    public async Task<IList<MangaEntity>> AddRange(IList<MangaInput> mangas)
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

        manga.PathToTitlePicture = $"{_localStorage.RelativePath}{manga.PathToTitlePicture}";

        foreach (var res in manga.PathToFoldersWithGlava)
        {
            res.LinkToFirstPicture = $"{_localStorage.RelativePath}{res.LinkToFirstPicture}";
        }

        return manga;
    }
    public async Task<PagedResult<List<MangaEntity>, object>> GetPagiantedMangaList(int sizeOfPage, int page)
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
    public async Task<List<MangaEntity>> FiltrationByDate(int year)
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
    public async Task<IList<MangaEntity>> FiltrationByName(string name)
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

    public async Task<bool> IsMangaExist(int mangaId)
    {
        using var dbContext = CreateDbContext();

        var manga = await dbContext.Mangas.FirstOrDefaultAsync(x => x.Id == mangaId);

        return manga != null;
    }
}
