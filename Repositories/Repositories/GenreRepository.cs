using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Repositories.Repositories.Base;

namespace Repositories.Repositories;

public class GenreRepository : BaseRepository<GenreEntity>, IGenreRepository
{
    private readonly IMangaRepository _mangaRepository;
    private readonly ILogger<GenreRepository> _logger;
    private readonly ITool _logTool;
    public GenreRepository(AppDBContent db, IMangaRepository mangaRepository,
        ILogger<GenreRepository> logger, ITool tool) : base(db)
    {
        _mangaRepository = mangaRepository;
        _logger = logger;
        _logTool = tool;
    }
    public async override Task<IList<GenreEntity>> GetAllAsync()
    {
        _logTool.NameOfMethod = nameof(GetAllAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin);

        var list = await _db.Genres.ToListAsync();

        if (list == null)
        {
            _logTool.WriteToLog(_logger, LogPosition.End, $"List of items is empty!");
            return new List<GenreEntity>();
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"list = {list}");

        return list;
    }
    public async override Task<GenreEntity> GetByIdAsync(string id)
    {
        _logTool.NameOfMethod = nameof(GetByIdAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {id}");

        var genre = await _db.Genres.Include(m => m.Mangas).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

        if (genre == null)
        {
            var errorMessage = $"The manga with id = { id } isn't contained in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);
            throw new Exception(errorMessage);
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"genre = {genre}");

        return genre;
    }
    public async override Task<GenreEntity> CreateAsync(GenreEntity item)
    {
        _logTool.NameOfMethod = nameof(CreateAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"item = {item}");

        var genre = await _db.Genres.FirstOrDefaultAsync(i => i.Name == item.Name);

        if (genre != null)
        {
            var errorMessage = $"The genre {item.Name} is contained in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);
            throw new Exception(errorMessage);
        }

        var result = await _db.Genres.AddAsync(item);

        if (result == null)
        {
            var errorMessage = $"The genre {item.Name} hasn't added in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);
            throw new Exception(errorMessage);
        }

        await _db.SaveChangesAsync();

        genre = await _db.Genres.Include(g => g.Mangas).FirstOrDefaultAsync(i => i.Name == item.Name);

        if (genre == null)
        {
            var errorMessage = $"The genre {item.Name} hasn't added in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);
            throw new Exception(errorMessage);
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"genre = {genre}");
        return genre;
    }
    public async override Task<GenreEntity> DeleteAsync(string id)
    {
        _logTool.NameOfMethod = nameof(DeleteAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"Id = {id}");

        var genre = await _db.Genres.Include(g => g.Mangas).FirstOrDefaultAsync(i => i.Id == id);

        if (genre == null)
        {
            var errorMessage = $"The genre with id = {id} isn't contained in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);
        }

        _db.Genres.Remove(genre);

        var mangaList = await _mangaRepository.GetAllAsync();

        mangaList.AsParallel().ForAll(m => m.Genres.Remove(genre));

        await _db.SaveChangesAsync();

        _logTool.WriteToLog(_logger, LogPosition.End, $"genre = {genre}");
        return genre;
    }
    public async override Task<GenreEntity> UpdateAsync(GenreEntity item)
    {
        _logTool.NameOfMethod = nameof(UpdateAsync);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"item = {item}");

        var result = _db.Genres.Update(item);

        if(result == null)
        {
            var errorMessage = $"The genre {item.Name} wan't updated";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            throw new Exception(errorMessage);
        }

        await _db.SaveChangesAsync();

        var genre = await _db.Genres.Include(g => g.Mangas).FirstOrDefaultAsync(i => i.Name == item.Name);

        if (genre == null)
        {
            var errorMessage = $"The genre {item.Name} hasn't updated in the database!";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);
            throw new Exception(errorMessage);
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"genre = {genre}");
        return genre;
    }
    public async override Task<IList<GenreEntity>> AddRange(IList<GenreEntity> items)
    {
        _logTool.NameOfMethod = nameof(AddRange);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"items = {items}");

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

        _logTool.WriteToLog(_logger, LogPosition.End, $"result = {result}");

        return result;
    }
    public async override Task<IList<GenreEntity>> GetCertainPage(int sizeOfPage, int page)
    {
        _logTool.NameOfMethod = nameof(GetCertainPage);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"sizeOfPage = {sizeOfPage}, page = {page} ");

        var list = await _db.Genres.Skip((page - 1) * sizeOfPage).Take(sizeOfPage).ToListAsync();

        if (list == null)
        {
            _logTool.WriteToLog(_logger, LogPosition.End, $"The list is empty");
            return new List<GenreEntity>();
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"list = {list}");

        return list;
    }
    public async override Task<IList<GenreEntity>> FiltrationByName(string name)
    {
        _logTool.NameOfMethod = nameof(FiltrationByName);
        _logTool.WriteToLog(_logger, LogPosition.Begin, $"name = {name}");

        var result = await _db.Genres.Where(i => i.Name.ToLower().Contains(name.ToLower())).ToListAsync();

        if (!result.Any())
        {
            var errorMessage = "No results";
            _logTool.WriteToLog(_logger, LogPosition.Error, errorMessage);

            return new List<GenreEntity>();
        }

        _logTool.WriteToLog(_logger, LogPosition.End, $"result = {result}");
        return result;
    }
}
