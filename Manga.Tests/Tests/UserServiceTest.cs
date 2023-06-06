using Data.Entities;
using Manga.Tests.Tests.Base;
using Manga.Tests.Utility;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Model.InputModel;
using Services.Services;
using Services.Services.Base;
using Xunit;

namespace Manga.Tests.Tests
{
    public class UserServiceTest : BaseTest
    {
        private Mock<IMangaService> _mangaServiceMock;
        private Mock<IGenreService> _genreServiceMock;
        private UserService Service;
        public UserServiceTest()
        {
            _mangaServiceMock = new Mock<IMangaService>();
            _genreServiceMock = new Mock<IGenreService>();

            using (var dbContext = CreateDbContext())
                dbContext.Database.EnsureDeleted();

            Service = new UserService(_genreServiceMock.Object, _mangaServiceMock.Object, options);
        }

        [Fact]
        public async void CreateUserTestRegularCase()
        {
            //Arrange
            var expectedResult = new UserEntity()
            {
                Id = 2,
                Email = "TestUser@gmail.com",
                Name = "TestUser",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            //Act
            var actualResult = await Service.CreateAsync(expectedResult);

            //Assert
            Assert.NotNull(actualResult);
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void UpdateUserTestChangeNameCase()
        {
            //Arrange
            SetupEnvironmentData();

            var userToUpdate = new UserInputModel()
            {
                Id = 1,
                Name = "UpdatedName"
            };

            var expectedResult = new UserEntity()
            {
                Id = 1,
                Email = "User@gmail.com",
                Name = "UpdatedName",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            //Act
            var result = await Service.UpdateUserAsync(userToUpdate);

            //Assert
            using var db = CreateDbContext();

            var actualResult = await db.Users.FirstOrDefaultAsync(u => u.Id == userToUpdate.Id);

            Assert.True(result);
            Assert.NotNull(actualResult);
            VerifyUser(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> DataForUserExistMethod =>
           new List<object[]>
           {
                new object[]
                {
                    Util.GetUser().Email,
                    Util.GetUser().Name,
                    true
                },
                new object[]
                {
                    "TestUserUser@gmail.com",
                    "User",
                    false
                },
           };
        [Theory]
        [MemberData(nameof(DataForUserExistMethod))]
        public async void IsUserExistsAsyncTestRegularCase(string email, string name, bool expectedResult)
        {
            //Arrange
            SetupEnvironmentData();

            //Act
            var actualResult = await Service.IsUserExistsAsync(email, name);

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }
        [Fact]
        public async void GetByIdAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = Util.GetUser();

            //Act
            var actualResult = await Service.GetByIdAsync(1);

            //Assert
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void GetUserByNameAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = Util.GetUser();

            //Act
            var actualResult = await Service.GetUserByNameAsync("TestUser");

            //Assert
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void GetUserByEmailAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = Util.GetUser();

            //Act
            var actualResult = await Service.GetUserByEmailAsync("User@gmail.com");

            //Assert
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void AddGenreToFavoriteAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = Util.GetUser();
            expectedResult.FavoriteGenresItems = new List<FavoriteGenreEntity>
            {
                new FavoriteGenreEntity(expectedResult, Util.GetGenre())
            };

            MockGenreService();

            //Act
            var result = await Service.AddGenreToFavoriteAsync("1", 1);

            //Assert
            using var db = CreateDbContext();
            var actualResult = await db.Users
                .Include(x => x.FavoriteGenresItems)
                    .ThenInclude(x => x.Genre)
                .FirstOrDefaultAsync(u => u.Id == expectedResult.Id);

            Assert.True(result);
            Assert.NotNull(actualResult);
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void AddMangaToFavoriteAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = Util.GetUser();
            expectedResult.FavoriteMangasItems = new List<FavoriteMangaEntity>
            {
                new FavoriteMangaEntity(expectedResult, Util.GetManga())
            };

            MockMangaService();

            //Act
            var result = await Service.AddMangaToFavoriteAsync("1", 1);

            //Assert
            using var db = CreateDbContext();
            var actualResult = await db.Users
                .Include(x => x.FavoriteMangasItems)
                    .ThenInclude(x => x.Manga)
                .FirstOrDefaultAsync(u => u.Id == expectedResult.Id);

            Assert.True(result);
            Assert.NotNull(actualResult);
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void RemoveGenreFromFavoriteAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = new UserEntity()
            {
                Id = 2,
                Email = "UserWithFavoriteItems@gmail.com",
                Name = "UserWithFavoriteItems",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            expectedResult.FavoriteMangasItems = new List<FavoriteMangaEntity>
            {
                new FavoriteMangaEntity(expectedResult, Util.GetManga())
            };

            MockGenreService();

            //Act
            var result = await Service.RemoveGenreFromFavoriteAsync("2", 1);

            //Assert
            using var db = CreateDbContext();
            var actualResult = await db.Users
                .Include(x => x.FavoriteGenresItems)
                    .ThenInclude(x => x.Genre)
                .Include(x => x.FavoriteMangasItems)
                    .ThenInclude(x => x.Manga)
                .FirstOrDefaultAsync(u => u.Id == expectedResult.Id);

            Assert.True(result);
            Assert.NotNull(actualResult);
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void RemoveMangaFromFavoriteAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = new UserEntity()
            {
                Id = 2,
                Email = "UserWithFavoriteItems@gmail.com",
                Name = "UserWithFavoriteItems",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            expectedResult.FavoriteGenresItems = new List<FavoriteGenreEntity>()
            {
                new FavoriteGenreEntity(expectedResult, Util.GetGenre()),
            };

            MockMangaService();

            //Act
            var result = await Service.RemoveGenreFromFavoriteAsync("2", 1);

            //Assert
            using var db = CreateDbContext();
            var actualResult = await db.Users
                .Include(x => x.FavoriteGenresItems)
                    .ThenInclude(x => x.Genre)
                .Include(x => x.FavoriteMangasItems)
                    .ThenInclude(x => x.Manga)
                .FirstOrDefaultAsync(u => u.Id == expectedResult.Id);

            Assert.True(result);
            Assert.NotNull(actualResult);
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void GetAllFavoriteMangasAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = new List<MangaEntity>()
            {
                Util.GetManga()
            };

            //Act
            var actualResult = await Service.GetAllFavoriteMangasAsync("2");

            //Assert
            for (int i = 0; i < expectedResult.Count; i++)
            {
                VerifyMangaEntity(expectedResult[i], actualResult[i]);
            }
        }
        [Fact]
        public async void GetAllFavoriteGenresAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = new List<GenreEntity>()
            {
                Util.GetGenre(),
            };

            //Act
            var actualResult = await Service.GetAllFavoriteGenresAsync("2");

            //Assert
            for (int i = 0; i < expectedResult.Count; i++)
            {
                VerifyGenre(expectedResult[i], actualResult[i]);
            }
        }

        private void MockGenreService()
        {
            _genreServiceMock
                 .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync(Util.GetGenre());
        }
        private void MockMangaService()
        {
            _mangaServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Util.GetManga());
        }
    }
}
