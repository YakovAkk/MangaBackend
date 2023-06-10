using Data.Database;
using Data.Entities;
using Manga.Tests.Utility;
using Microsoft.EntityFrameworkCore;
using Services.Model.ViewModel;
using Xunit;

namespace Manga.Tests.Tests.Base
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

            dbInit.Users.Add(Util.GetUser());

            var manga = Util.GetManga();

            dbInit.Mangas.Add(manga);

            var genres = new List<GenreEntity>()
            {
                Util.GetGenre(),
                new GenreEntity() {Name = "aaa",
                    Mangas = new List<MangaEntity> ()
                    {
                        manga
                    }
                }
            };

            dbInit.Genres.AddRange(genres);

            var userWithFavoriteItems = new UserEntity()
            {
                Id = 2,
                Email = "UserWithFavoriteItems@gmail.com",
                Name = "UserWithFavoriteItems",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            userWithFavoriteItems.FavoriteGenresItems = new List<FavoriteGenreEntity>()
            {
                new FavoriteGenreEntity(userWithFavoriteItems, genres[0]),
            };
            userWithFavoriteItems.FavoriteMangasItems = new List<FavoriteMangaEntity>()
            {
                new FavoriteMangaEntity(userWithFavoriteItems, manga)
            };

            dbInit.Users.Add(userWithFavoriteItems);

            await dbInit.SaveChangesAsync();
        }

        protected void VerifyMangaEntity(MangaEntity expectedResult, MangaEntity actualResult)
        {
            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.AgeRating, actualResult.AgeRating);
            Assert.Equal(expectedResult.Author, actualResult.Author);
            Assert.Equal(expectedResult.Description, actualResult.Description);
            Assert.Equal(expectedResult.PathToTitlePicture, actualResult.PathToTitlePicture);
            Assert.Equal(expectedResult.ReleaseYear, actualResult.ReleaseYear);
            Assert.Equal(expectedResult.NumbetOfChapters, actualResult.NumbetOfChapters);
        }

        protected void VerifyMangaViewModel(MangaViewModel expectedResult, MangaViewModel actualResult)
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

            if (expectedResult.Mangas != null)
                for (int i = 0; i < expectedResult.Mangas.Count; i++)
                    VerifyMangaEntity(expectedResult.Mangas[i], actualResult.Mangas[i]);
        }

        protected void VerifyUser(UserEntity expectedResult, UserEntity actualResult)
        {
            Assert.Equal(expectedResult.Id, actualResult.Id);
            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.Email, actualResult.Email);
            Assert.Equal(expectedResult.PasswordHash, actualResult.PasswordHash);
            Assert.Equal(expectedResult.PasswordSalt, actualResult.PasswordSalt);
            Assert.Equal(expectedResult.DeviceToken, actualResult.DeviceToken);

            if (expectedResult.FavoriteGenres != null)
                for (int i = 0; i < expectedResult.FavoriteGenres.Count; i++)
                {
                    VerifyGenre(expectedResult.FavoriteGenres[i], actualResult.FavoriteGenres[i]);
                }

            if (expectedResult.FavoriteMangas != null)
                for (int i = 0; i < expectedResult.FavoriteMangas.Count; i++)
                {
                    VerifyMangaEntity(expectedResult.FavoriteMangas[i], actualResult.FavoriteMangas[i]);
                }
        }
    }
}
