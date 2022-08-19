using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Database
{
    public class AppDBContent : DbContext
    {
        public DbSet<GlavaMangaModel> glavaManga { get; set; }
        public DbSet<MangaModel> Mangas { get; set; }
        public DbSet<GenreModel> Genres { get; set; }

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
}
