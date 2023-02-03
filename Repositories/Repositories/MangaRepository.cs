using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Repositories.Repositories;

public class MangaRepository : BaseRepository<MangaEntity>, IMangaRepository
{
    public MangaRepository(AppDBContent db) : base(db)
    {
    }
    public async override Task<IList<MangaEntity>> GetAllAsync()
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
    public async override Task<MangaEntity> DeleteAsync(string id)
    {
        var manga = await _db.Mangas.FirstOrDefaultAsync(i => i.Id == id);

        if (manga == null)
        {
            var errorMessage = $"The manga with id = {id} isn't contained in the database!";

            throw new Exception(errorMessage);
        }

        _db.Mangas.Remove(manga);

        await _db.SaveChangesAsync();
    
        return manga;
    }
    public async override Task<MangaEntity> CreateAsync(MangaEntity item)
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
    public async override Task<MangaEntity> GetByIdAsync(string Id)
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
    public async override Task<MangaEntity> UpdateAsync(MangaEntity item)
    {
      
        if (item == null)
        {
            var errorMessage = "Item was null";

            throw new Exception(errorMessage);
        }

        var result = _db.Mangas.Update(item);

        if (result == null)
        {
            var errorMessage = $"The manga {item.Name} hasn't updated in the database!";
         
            throw new Exception(errorMessage);
        }

        await _db.SaveChangesAsync();

        var manga = await _db.Mangas.FirstOrDefaultAsync(i => i.Name == item.Name);

        if (manga == null)
        {
            var errorMessage = $"The manga {item.Name} hasn't added in the database!";

            throw new Exception(errorMessage);
        }

        return manga;
    }
    public async override Task<IList<MangaEntity>> AddRange(IList<MangaEntity> items)
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
    public async override Task<IList<MangaEntity>> GetCertainPage(int sizeOfPage, int page)
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
    public async override Task<IList<MangaEntity>> FiltrationByName(string name)
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
