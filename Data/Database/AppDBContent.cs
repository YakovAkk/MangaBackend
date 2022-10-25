using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Database;

public class AppDBContent : DbContext
{
    public DbSet<GlavaMangaEntity> GlavaManga { get; set; }
    public DbSet<MangaEntity> Mangas { get; set; }
    public DbSet<GenreEntity> Genres { get; set; }

    public AppDBContent(DbContextOptions<AppDBContent> options) : base(options)
    {
        try
        {
            Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}

