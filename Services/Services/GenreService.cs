using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
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

        if (genre == null)
        {
            var errorMessage = $"The genre doesn't exist!";
            throw new Exception(errorMessage);
        }

        return genre;
    }
    public async Task<IList<GenreEntity>> GetCertainPage(string sizeOfPage, string page)
    {
        int pageSize, numberOfPage;

        if (!ValidatorService.IsValidPageAndPageSize(sizeOfPage, page, out pageSize, out numberOfPage))
        {
            throw new Exception("Parameters aren't valid");
        }

        using var dbContext = CreateDbContext();

        var list = await dbContext.Genres
             .Skip((numberOfPage - 1) * pageSize)
             .Take(pageSize)
             .ToListAsync();

        return list;
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
