using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Repositories.Repositories.Base;

namespace Repositories.Repositories;

public class MangaRepository : BaseRepository<MangaEntity>, IMangaRepository
{

    private readonly ILogger<GenreRepository> _logger;
    private readonly ILogsTool _logTool;

    public MangaRepository(AppDBContent db, ILogger<GenreRepository> logger, ILogsTool tool) : base(db)
    {
        _logger = logger;
        _logTool = tool;
    }
    public async override Task<IList<MangaEntity>> GetAllAsync()
    {
        _logTool.NameOfMethod = nameof(GetAllAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin);

        var list = await _db.Mangas.Include(m => m.Genres).AsNoTracking()
            .Include(m => m.PathToFoldersWithGlava).ToListAsync();

        if (list == null)
        {
            _logTool.WriteToLog(_logger, LogPosition.End, $"List of items is empty!");
            return new List<MangaEntity>();
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"list = {list}");

        return list;
    }
    public async override Task<MangaEntity> DeleteAsync(string id)
    {
        _logTool.NameOfMethod = nameof(DeleteAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {id}");

        var manga = await _db.Mangas.Include(m => m.Genres)
            .AsNoTracking().Include(m => m.PathToFoldersWithGlava).FirstOrDefaultAsync(i => i.Id == id);

        if (manga == null)
        {
            var errorMessage = $"The manga with id = {id} isn't contained in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);
        }

        _db.Mangas.Remove(manga);

        await _db.SaveChangesAsync();
        _logTool.WriteToLog(_logger, LogPosition.End, $"manga = {manga}");

        return manga;
    }
    public async override Task<MangaEntity> CreateAsync(MangaEntity item)
    {
        _logTool.NameOfMethod = nameof(CreateAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"item = {item}");

        var manga = await _db.Mangas.FirstOrDefaultAsync(i => i.Name == item.Name);

        if (manga != null)
        {
            var errorMessage = $"The manga {item.Name} is contained in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);
        }

        var result = await _db.Mangas.AddAsync(item);

        if (result == null)
        {
            var errorMessage = $"The manga {item.Name} hasn't added in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);
        }

        await _db.SaveChangesAsync();

        manga = await _db.Mangas.Include(m => m.Genres)
            .AsNoTracking().Include(m => m.PathToFoldersWithGlava).FirstOrDefaultAsync(i => i.Name == item.Name);

        if (manga == null)
        {
            var errorMessage = $"The manga {item.Name} hasn't added in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"manga = {manga}");

        return manga;
    }
    public async override Task<MangaEntity> GetByIdAsync(string Id)
    {
        _logTool.NameOfMethod = nameof(GetByIdAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");

        var manga = await _db.Mangas.Include(m => m.Genres)
            .Include(m => m.PathToFoldersWithGlava).FirstOrDefaultAsync(i => i.Id == Id);

        if (manga == null)
        {
            var errorMessage = $"The manga with id = {Id} isn't contained in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);
            throw new Exception(errorMessage);
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"manga = {manga}");

        return manga;
    }
    public async override Task<MangaEntity> UpdateAsync(MangaEntity item)
    {
        _logTool.NameOfMethod = nameof(UpdateAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"item = {item}");

        if (item == null)
        {
            var errorMessage = "Item was null";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);
        }

        var result = _db.Mangas.Update(item);

        if (result == null)
        {
            var errorMessage = $"The manga {item.Name} hasn't updated in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);
        }

        await _db.SaveChangesAsync();

        var manga = await _db.Mangas.Include(m => m.Genres).AsNoTracking()
            .Include(m => m.PathToFoldersWithGlava).FirstOrDefaultAsync(i => i.Name == item.Name);

        if (manga == null)
        {
            var errorMessage = $"The manga {item.Name} hasn't added in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"manga = {manga}");

        return manga;
    }
    public async override Task<IList<MangaEntity>> AddRange(IList<MangaEntity> items)
    {
        _logTool.NameOfMethod = nameof(AddRange);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"items = {items}");

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

        _logTool.WriteToLog(_logger, LogPosition.End, $"result = {result}");

        return result;
    }
    public async override Task<IList<MangaEntity>> GetCertainPage(int sizeOfPage, int page)
    {
        _logTool.NameOfMethod = nameof(GetCertainPage);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"sizeOfPage = {sizeOfPage}, page = {page} ");

        var list = await _db.Mangas.Include(m => m.Genres).AsNoTracking()
            .Include(m => m.PathToFoldersWithGlava).Skip((page -1) * sizeOfPage).Take(sizeOfPage).ToListAsync();

        if (list == null)
        {
            _logTool.WriteToLog(_logger, LogPosition.End, $"The list is empty");

            return new List<MangaEntity>();
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"list = {list}");

        return list;
    }
    public async override Task<IList<MangaEntity>> GetAllFavoriteAsync()
    {
        _logTool.NameOfMethod = nameof(GetAllFavoriteAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin);

        var list = await _db.Mangas.Where(i => i.IsFavorite).ToListAsync();

        if (list == null)
        {
            _logTool.WriteToLog(_logger, LogPosition.End, $"The list is empty");

            return new List<MangaEntity>();
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"list = {list}");

        return list;
    }
    public async override Task<MangaEntity> AddToFavorite(string Id)
    {
        _logTool.NameOfMethod = nameof(AddToFavorite);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");

        try
        {
            var manga = await GetByIdAsync(Id);

            manga.IsFavorite = true;

            await UpdateAsync(manga);

            await _db.SaveChangesAsync();

            return await GetByIdAsync(manga.Id);
        }
        catch (Exception ex)
        {
            _logTool.WriteToLog(_logger, LogPosition.Error, ex.Message);

            throw new Exception(ex.Message);
        }
    }
    public async override Task<MangaEntity> RemoveFavorite(string Id)
    {
        _logTool.NameOfMethod = nameof(RemoveFavorite);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {Id}");

        try
        {
            var manga = await GetByIdAsync(Id);

            manga.IsFavorite = false;

            await UpdateAsync(manga);

            await _db.SaveChangesAsync();

            return await GetByIdAsync(manga.Id);
        }
        catch (Exception ex)
        {
            _logTool.WriteToLog(_logger, LogPosition.Error, ex.Message);

            throw new Exception(ex.Message);
        }
    }
    public async override Task<IList<MangaEntity>> FiltrationByName(string name)
    {
        _logTool.NameOfMethod = nameof(FiltrationByName);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"name = {name}");

        var result = await _db.Mangas
            .Include(i=> i.PathToFoldersWithGlava)
            .Where(i => i.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();

        if (!result.Any())
        {
            var errorMessage = "No results";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            return new List<MangaEntity>();
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"result = {result}");

        return result;
    }
    public async Task<List<MangaEntity>> FiltrationByDate(int year)
    {
        _logTool.NameOfMethod = nameof(FiltrationByName);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"year = {year}");

        var result = await _db.Mangas.Where(i => i.ReleaseYear > year).ToListAsync();

        if(result == null)
        {
            var errorMessage = "No results";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            return new List<MangaEntity>();
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"result = {result}");

        return result;
    }
}
