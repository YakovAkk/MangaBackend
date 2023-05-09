using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Manga.Tests.Base
{
    public class BaseTest
    {
        protected static int genreId = 1;

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
                ReleaseYear = 2000,
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

        protected void VerifyManga(MangaEntity expectedResult, MangaEntity actualResult)
        {
            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.AgeRating, actualResult.AgeRating);
            Assert.Equal(expectedResult.Author, actualResult.Author);
            Assert.Equal(expectedResult.Description, actualResult.Description);
            Assert.Equal(expectedResult.PathToTitlePicture, actualResult.PathToTitlePicture);
            Assert.Equal(expectedResult.ReleaseYear, actualResult.ReleaseYear);
            Assert.Equal(expectedResult.NumbetOfChapters, actualResult.NumbetOfChapters);
        }

        protected void VerifyGenre(GenreEntity expectedResult, GenreEntity actualResult)
        {
            Assert.Equal(expectedResult.Name, actualResult.Name);

            if(expectedResult.Mangas != null)
                for (int i = 0; i < expectedResult.Mangas.Count; i++)
                    VerifyManga(expectedResult.Mangas[i], actualResult.Mangas[i]);
        }
    }
}
