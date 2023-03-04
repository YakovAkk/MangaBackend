using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Database;

public class AppDBContext : DbContext
{
    private readonly DbContextOptions<AppDBContext> dbContextOptions;
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<GlavaMangaEntity> GlavaManga { get; set; }
    public DbSet<MangaEntity> Mangas { get; set; }
    public DbSet<GenreEntity> Genres { get; set; }

    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
        try
        {
            dbContextOptions = options;
            Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public DbContext CreateDbContext()
    {
        return (DbContext)Activator.CreateInstance(typeof(DbContext), dbContextOptions);
    }
}

