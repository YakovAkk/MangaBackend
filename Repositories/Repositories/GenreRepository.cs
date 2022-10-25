using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Repositories.Repositories;

public class GenreRepository : BaseRepository<GenreEntity>, IGenreRepository
{
    private readonly IMangaRepository _mangaRepository;

    public GenreRepository(AppDBContent db, IMangaRepository mangaRepository) : base(db)
    {
        _mangaRepository = mangaRepository;
    }
    public async override Task<IList<GenreEntity>> GetAllAsync()
    {
        var list = await _db.Genres.ToListAsync();

        if (list == null)
        {
            return new List<GenreEntity>();
        }

        return list;
    }
    public async override Task<GenreEntity> GetByIdAsync(string id)
    {

        var genre = await _db.Genres.Include(m => m.Mangas).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

        if (genre == null)
        {
            throw new Exception($"The manga with id = {id} isn't contained in the database!");
        }

        return genre;
    }
    public async override Task<GenreEntity> CreateAsync(GenreEntity item)
    {

        var genre = await _db.Genres.FirstOrDefaultAsync(i => i.Name == item.Name);

        if (genre != null)
        {
            throw new Exception($"The genre {item.Name} is contained in the database!");
        }

        var result = await _db.Genres.AddAsync(item);

        if (result == null)
        {
            throw new Exception($"The genre {item.Name} hasn't added in the database!");
        }

        await _db.SaveChangesAsync();

        genre = await _db.Genres.Include(g => g.Mangas).FirstOrDefaultAsync(i => i.Name == item.Name);

        if (genre == null)
        {
            throw new Exception($"The genre {item.Name} hasn't added in the database!");
        }

        return genre;
    }
    public async override Task<GenreEntity> DeleteAsync(string id)
    {
        var genre = await _db.Genres.Include(g => g.Mangas).FirstOrDefaultAsync(i => i.Id == id);

        if (genre == null)
        {
            throw new Exception($"The genre with id = {id} isn't contained in the database!");
        }

        _db.Genres.Remove(genre);

        var mangaList = await _mangaRepository.GetAllAsync();

        mangaList.AsParallel().ForAll(m => m.Genres.Remove(genre));

        await _db.SaveChangesAsync();

        return genre;
    }
    public async override Task<GenreEntity> UpdateAsync(GenreEntity item)
    {
        var result = _db.Genres.Update(item);

        if(result == null)
        {
            throw new Exception($"The genre {item.Name} wan't updated");
        }

        await _db.SaveChangesAsync();

        var genre = await _db.Genres.Include(g => g.Mangas).FirstOrDefaultAsync(i => i.Name == item.Name);

        if (genre == null)
        {
            throw new Exception($"The genre {item.Name} hasn't updated in the database!");
        }

        return genre;
    }
    public async override Task<IList<GenreEntity>> AddRange(IList<GenreEntity> items)
    {
        var result = new List<GenreEntity>();

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
    public async override Task<IList<GenreEntity>> GetCertainPage(int sizeOfPage, int page)
    {
        var list = await _db.Genres.Skip((page - 1) * sizeOfPage).Take(sizeOfPage).ToListAsync();

        if (list == null)
        {
            return new List<GenreEntity>();
        }

        return list;
    }
    public async override Task<IList<GenreEntity>> GetAllFavoriteAsync()
    {
        var list = await _db.Genres.Where(i => i.IsFavorite).ToListAsync();

        if (list == null)
        {
            return new List<GenreEntity>();
        }

        return list;
    }
    public async override Task<GenreEntity> AddToFavorite(string Id)
    {
        try
        {
            var genre = await GetByIdAsync(Id);

            genre.IsFavorite = true;

            await UpdateAsync(genre);

            await _db.SaveChangesAsync();

            return await GetByIdAsync(genre.Id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async override Task<GenreEntity> RemoveFavorite(string Id)
    {
        try
        {
            var genre = await GetByIdAsync(Id);

            genre.IsFavorite = false;

            await UpdateAsync(genre);

            await _db.SaveChangesAsync();

            return await GetByIdAsync(genre.Id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async override Task<IList<GenreEntity>> FiltrationByName(string name)
    {
        var result = await _db.Genres.Where(i => i.Name.ToLower().Contains(name.ToLower())).ToListAsync();

        if (!result.Any())
        {
            throw new Exception("No results");
        }

        return result;
    }
}
