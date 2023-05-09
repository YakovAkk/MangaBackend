using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Manga.Tests.Base
{
    public class BaseTest
    {
        protected DbContextOptions<AppDBContext> options;
        public BaseTest()
        {
            options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase(GetType().Name).Options;

            using (var dbContext = CreateDbContext())
                dbContext.Database.EnsureDeleted();
        }
        protected AppDBContext CreateDbContext() => new AppDBContext(options);

        protected async void SetupEnvironmentData()
        {
            using var dbInit = CreateDbContext();

            var user = new UserEntity()
            {
                Email = "User@gmail.com",
                Name = "TestUser",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            dbInit.Users.Add(user);

            var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
            {
                new GlavaMangaEntity()
                {
                    NumberOfGlava = 1,
                    LinkToFirstPicture = "manga/tokyoghoul/glava1/1.jpg",
                    NumberOfPictures = 46
                }
            };

            var manga = new MangaEntity()
            {
                Name = "TestManga",
                AgeRating = "18+",
                Description = "Description",
                Author = "Author",
                NumbetOfChapters = 1,
                PathToFoldersWithGlava = new List<GlavaMangaEntity> { new GlavaMangaEntity()
                {
                    LinkToFirstPicture = "./",
                    NumberOfGlava = 2,
                    NumberOfPictures = 1,
                }},
                PathToTitlePicture = "./"
            };

            dbInit.Mangas.Add(manga);

            var genres = new List<GenreEntity>()
            {
                new GenreEntity() {Name = "genre1"},
                new GenreEntity() {Name = "aaa",
                    Mangas = new List<MangaEntity> ()
                    {
                        manga
                    }
                }
            };

            dbInit.Genres.AddRange(genres);

            await dbInit.SaveChangesAsync();
        }
    }
}
