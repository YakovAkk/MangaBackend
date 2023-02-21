using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Repositories.Repositories;

public class GenreRepository : DbService<AppDBContext>, IGenreRepository
{
    private readonly IMangaRepository _mangaRepository;
    private readonly AppDBContext _db;
    public GenreRepository(AppDBContext db, IMangaRepository mangaRepository, 
        DbContextOptions<AppDBContext> dbContextOptions) : base(dbContextOptions)
    {
        _mangaRepository = mangaRepository;
        _db = db;
    }
    public async Task<IList<GenreEntity>> GetAllAsync()
    {
        using var dbContext = CreateDbContext();
        var list = await dbContext.Genres.ToListAsync();

        if (list == null)
        {
            return new List<GenreEntity>();
        }
        return list;
    }
    public async Task<GenreEntity> GetByIdAsync(string id)
    {
        var genre = await _db.Genres
            .Include(m => m.Mangas)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (genre == null)
        {
            var errorMessage = $"The manga with id = { id } isn't contained in the database!";
            throw new Exception(errorMessage);
        }

        return genre;
    } 
}
