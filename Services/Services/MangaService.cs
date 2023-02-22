using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Services.Base;
using Services.Storage.Base;
using System;
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

        if (result == null)
        {
            return new List<MangaEntity>();
        }

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
        var result = await _mangaRepository.GetByIdAsync(Id);

        result.PathToTitlePicture = $"{_localStorage.RelativePath}{result.PathToTitlePicture}";

        foreach (var res in result.PathToFoldersWithGlava)
        {
            res.LinkToFirstPicture = $"{_localStorage.RelativePath}{res.LinkToFirstPicture}";
        }

        return result;
    }
    public async Task<IList<MangaEntity>> GetCertainPage(string sizeOfPage, string page)
    {
        int pageSize, numberOfPage;

        if (!ValidatorService.IsValidPageAndPageSize(sizeOfPage, page, out pageSize, out numberOfPage))
        {
            throw new Exception("Parameters aren't valid");
        }

        return await _mangaRepository.GetCertainPage(pageSize, numberOfPage);
    }
    public async Task<List<MangaEntity>> FiltrationByDate(string year)
    {
        int yearnum = 0;

        if (!ValidatorService.IsValidYear(year, out yearnum))
        {
            throw new Exception("Parameters aren't valid");
        }

        return await _mangaRepository.FiltrationByDate(yearnum);
    }
    public async Task<IList<MangaEntity>> FiltrationByName(string name)
    {
        return await _mangaRepository.FiltrationByName(name);
    }
}
