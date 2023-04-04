using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Core;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Services.Base;
using ValidateService.Validate;

namespace Services.Services;

public class GenreService : DbService<AppDBContext>, IGenreService
{
    public GenreService(DbContextOptions<AppDBContext> dbContextOptions) 
        : base(dbContextOptions) { }
    
    public async Task<IList<GenreEntity>> AddRange(IList<GenreInput> genres)
    {
        using var dbContext = CreateDbContext();

        foreach (var genre in genres)
        {
            var genreEntity = genre.toEntity();

            if(dbContext.Genres.SingleOrDefault(x => x.Name == genreEntity.Name) == null)
                dbContext.Genres.Add(genreEntity);
        }

        await dbContext.SaveChangesAsync();

        return await GetAllAsync();
    }
    public async Task<IList<GenreEntity>> GetAllAsync()
    {
        using var dbContext = CreateDbContext();

        var list = await dbContext.Genres
            .ToListAsync();

        return list;
    }
    public async Task<GenreEntity> GetByIdAsync(string id)
    {
        using var dbContext = CreateDbContext();

        var genre = await dbContext.Genres
            .Include(m => m.Mangas)
            .FirstOrDefaultAsync(i => i.Id == id);

        foreach (var manga in genre.Mangas)
        {
            manga.ClearGenre();
            manga.ClearPathToFoldersWithGlava();
        }

        if (genre == null)
        {
            var errorMessage = $"The genre doesn't exist!";
            throw new Exception(errorMessage);
        }

        return genre;
    }
    public async Task<PagedResult<List<GenreEntity>, object>> GetPaginatedGenreList(string sizeOfPage, string page)
    {
        int pageSize, numberOfPage;

        if (!ValidatorService.IsValidPageAndPageSize(sizeOfPage, page, out pageSize, out numberOfPage))
        {
            throw new Exception("Parameters aren't valid");
        }

        IQueryable<GenreEntity> Query(AppDBContext dbContext)
        {
            return dbContext.Genres;
        }

        int count;
        List<GenreEntity> genresResult;

        using (var contextPool = new ContextPool<AppDBContext>(() => CreateDbContext()))
        {
            var dataTask = Query(contextPool.NewContext())
                 .Skip((numberOfPage - 1) * pageSize)
                 .Take(pageSize)
            .ToListAsync();

            var countTask = Query(contextPool.NewContext()).CountAsync();

            await Task.WhenAll(dataTask, countTask);

            genresResult = dataTask.Result;
            count = countTask.Result;
        }

        return new PagedResult<List<GenreEntity>, object>(count, genresResult, null);
    }
    public async Task<IList<GenreEntity>> FiltrationByName(string name)
    {
        using var dbContext = CreateDbContext();

        var list = await dbContext.Genres
            .Where(i => i.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();

        return list;
    }
}
