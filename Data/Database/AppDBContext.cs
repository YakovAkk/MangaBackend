using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Database;

public class AppDBContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<GlavaMangaEntity> GlavaManga { get; set; }
    public DbSet<MangaEntity> Mangas { get; set; }
    public DbSet<GenreEntity> Genres { get; set; }
    public DbSet<FavoriteGenreEntity> FavoriteGenres { get; set; }
    public DbSet<FavoriteMangaEntity> FavoriteMangas { get; set; }
    public DbSet<RememberReadingItem> RememberReadingItems { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
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

