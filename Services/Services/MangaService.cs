using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Services.Base;
using Services.Storage.Base;
using ValidateService.Validate;

namespace Services.Services;

public class MangaService : DbService<AppDBContext>, IMangaService
{
    private readonly ILocalStorage _localStorage;
    private readonly IGenreRepository _genreRepository;
    private readonly IMangaRepository _mangaRepository;
    public MangaService(
        IMangaRepository repository,
        IGenreRepository genreRepository,
        ILocalStorage localStorage,
        DbContextOptions<AppDBContext> dbContextOptions) : base(dbContextOptions)
    {
        _genreRepository = genreRepository;
        _mangaRepository = repository;
        _localStorage = localStorage;
    }
    
    public async Task<IList<MangaEntity>> GetAllAsync()
    {
        using var dbContext = CreateDbContext();

        var result = await dbContext.Mangas.ToListAsync();

        foreach (var item in result)
        {
            item.PathToTitlePicture = $"{_localStorage.RelativePath}{item.PathToTitlePicture}";
            foreach (var res in item.PathToFoldersWithGlava)
            {
                res.LinkToFirstPicture = $"{_localStorage.RelativePath}{res.LinkToFirstPicture}";
            }
        }

        return result;
    }
    public async Task<IList<MangaEntity>> AddRange(IList<MangaInput> mangas)
    {
        using var dbContext = CreateDbContext();

        var genresNames = mangas.SelectMany(x => x.Genres_names);

        var genres = dbContext.Genres.Where(x => genresNames.Contains(x.Name)).ToList();

        foreach (var manga in mangas)
        {
            var genresForManga = genres.Where(x => manga.Genres_names.Contains(x.Name)).ToList();

            var mangaEntity = manga.toEntity(genresForManga);

            if (dbContext.Mangas.SingleOrDefault(x => x.Name == mangaEntity.Name) == null)
                dbContext.Mangas.Add(mangaEntity);
        }

        await dbContext.SaveChangesAsync();

        return await GetAllAsync();
    }
    public async Task<MangaEntity> GetByIdAsync(string Id)
    {
        using var dbContext = CreateDbContext();

        var manga = await dbContext.Mangas
            .Include(m => m.Genres)
            .Include(m => m.PathToFoldersWithGlava)
            .FirstOrDefaultAsync(i => i.Id == Id);

        if (manga == null)
        {
            var errorMessage = $"The manga with id = {Id} isn't contained in the database!";
            throw new Exception(errorMessage);
        }

        manga.PathToTitlePicture = $"{_localStorage.RelativePath}{manga.PathToTitlePicture}";

        foreach (var res in manga.PathToFoldersWithGlava)
        {
            res.LinkToFirstPicture = $"{_localStorage.RelativePath}{res.LinkToFirstPicture}";
        }

        return manga;
    }
    public async Task<IList<MangaEntity>> GetCertainPage(string sizeOfPage, string page)
    {
        int pageSize, numberOfPage;

        if (!ValidatorService.IsValidPageAndPageSize(sizeOfPage, page, out pageSize, out numberOfPage))
        {
            throw new Exception("Parameters aren't valid");
        }

        using var dbContext = CreateDbContext();

        var list = await dbContext.Mangas
            .Include(m => m.Genres)
            .AsNoTracking()
            .Include(m => m.PathToFoldersWithGlava)
            .Skip((numberOfPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return list;
    }
    public async Task<List<MangaEntity>> FiltrationByDate(string year)
    {
        int yearNum = 0;

        if (!ValidatorService.IsValidYear(year, out yearNum))
        {
            throw new Exception("Parameters aren't valid");
        }

        using var dbContext = CreateDbContext();

        var result = await dbContext.Mangas
            .Include(m => m.Genres)
            .AsNoTracking()
            .Include(i => i.PathToFoldersWithGlava)
            .Where(i => i.ReleaseYear > yearNum)
            .ToListAsync();

        return result;
    }
    public async Task<IList<MangaEntity>> FiltrationByName(string name)
    {
        using var dbContext = CreateDbContext();

        var result = await dbContext.Mangas
            .Include(m => m.Genres)
            .AsNoTracking()
            .Include(i => i.PathToFoldersWithGlava)
            .Where(i => i.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();

        return result;
    }
}
