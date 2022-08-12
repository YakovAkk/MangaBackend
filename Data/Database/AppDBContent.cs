using Data.Models;
using Microsoft.EntityFrameworkCore;


namespace Data.Database
{
    public class AppDBContent : DbContext
    {
        public DbSet<GlavaMangaModel> glavaManga { get; set; }
        public DbSet<MangaModel> Mangas { get; set; }
        public DbSet<GenreModel> Genres { get; set; }

        public AppDBContent(DbContextOptions<AppDBContent> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
