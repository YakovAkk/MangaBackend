using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Repositories.Repositories;

public class MangaRepository : IMangaRepository
{
    private readonly AppDBContext _db;
    public MangaRepository(AppDBContext db)
    {
        _db = db;
    }
    public async Task<IList<MangaEntity>> GetAllAsync()
    {
        var list = await _db.Mangas
            .Include(m => m.Genres)
            .AsNoTracking()
            .Include(m => m.PathToFoldersWithGlava)
            .ToListAsync();

        if (list == null)
        {
            return new List<MangaEntity>();
        }

        return list;
    }
    public async Task<MangaEntity> CreateAsync(MangaEntity item)
    {
        var manga = await _db.Mangas.FirstOrDefaultAsync(i => i.Name == item.Name);

        if (manga != null)
        {
            var errorMessage = $"The manga {item.Name} is contained in the database!";
            throw new Exception(errorMessage);
        }

        var result = await _db.Mangas.AddAsync(item);

        if (result == null)
        {
            var errorMessage = $"The manga {item.Name} hasn't added in the database!";
            throw new Exception(errorMessage);
        }

        await _db.SaveChangesAsync();

        manga = await _db.Mangas.FirstOrDefaultAsync(i => i.Name == item.Name);

        if (manga == null)
        {
            var errorMessage = $"The manga {item.Name} hasn't added in the database!";

            throw new Exception(errorMessage);
        }
        return manga;
    }
    public async Task<MangaEntity> GetByIdAsync(string Id)
    {
        var manga = await _db.Mangas
            .Include(m => m.Genres)
            .Include(m => m.PathToFoldersWithGlava)
            .FirstOrDefaultAsync(i => i.Id == Id);

        if (manga == null)
        {
            var errorMessage = $"The manga with id = {Id} isn't contained in the database!";
            throw new Exception(errorMessage);
        }

        return manga;
    }
    public async Task<IList<MangaEntity>> AddRange(IList<MangaEntity> items)
    {
        var result = new List<MangaEntity>();

        foreach (var item in items)
        {
            try
            {
                var model = await CreateAsync(item);
                result.Add(model);
            }
            catch (Exception)
            {
                continue;
            }
 
        };

        return result;
    }
    public async Task<IList<MangaEntity>> GetCertainPage(int sizeOfPage, int page)
    {
        var list = await _db.Mangas
            .Include(m => m.Genres)
            .AsNoTracking()
            .Include(m => m.PathToFoldersWithGlava)
            .Skip((page -1) * sizeOfPage)
            .Take(sizeOfPage)
            .ToListAsync();

        if (list == null)
        {
            return new List<MangaEntity>();
        }

        return list;
    }
    public async Task<IList<MangaEntity>> FiltrationByName(string name)
    {
        var result = await _db.Mangas
            .Include(m => m.Genres)
            .AsNoTracking()
            .Include(i=> i.PathToFoldersWithGlava)
            .Where(i => i.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();

        if (!result.Any())
        {
            return new List<MangaEntity>();
        }

        return result;
    }
    public async Task<List<MangaEntity>> FiltrationByDate(int year)
    {
        var result = await _db.Mangas
            .Include(m => m.Genres)
            .AsNoTracking()
            .Include(i => i.PathToFoldersWithGlava)
            .Where(i => i.ReleaseYear > year)
            .ToListAsync();

        if(result == null)
        {     
            return new List<MangaEntity>();
        }

        return result;
    }
}
