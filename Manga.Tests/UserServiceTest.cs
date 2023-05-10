using Data.Entities;
using Manga.Tests.Base;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Model.InputModel;
using Services.Services;
using Services.Services.Base;
using System.Xml.Linq;
using Xunit;

namespace Manga.Tests
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
            using var db = CreateDbContext();

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
                    "User@gmail.com",
                    "TestUser",
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
        public async void IsUserExistsAsyncTestChangeRegularCase(string email, string name, bool expectedResult)
        {
            //Arrange
            SetupEnvironmentData();

            //Act
            var actualResult = await Service.IsUserExistsAsync(email, name);

            //Assert
            Assert.Equal(expectedResult,actualResult);
        }
        [Fact]
        public async void GetByIdAsyncTestChangeRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = new UserEntity()
            {
                Id = 1,
                Email = "User@gmail.com",
                Name = "TestUser",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            //Act
            var actualResult = await Service.GetByIdAsync(1);

            //Assert
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void GetUserByNameAsyncTestChangeRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = new UserEntity()
            {
                Id = 1,
                Email = "User@gmail.com",
                Name = "TestUser",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            //Act
            var actualResult = await Service.GetUserByNameAsync("TestUser");

            //Assert
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void GetUserByEmailAsyncTestChangeRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = new UserEntity()
            {
                Id = 1,
                Email = "User@gmail.com",
                Name = "TestUser",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            //Act
            var actualResult = await Service.GetUserByEmailAsync("User@gmail.com");

            //Assert
            VerifyUser(expectedResult, actualResult);
        }
        [Fact]
        public async void AddGenreToFavoriteAsyncTestChangeRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = new UserEntity()
            {
                Id = 1,
                Email = "User@gmail.com",
                Name = "TestUser",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 },
                FavoriteGenres = new List<GenreEntity> 
                { 
                    new GenreEntity() 
                    {
                        Name = "genre1"
                    } 
                }
            };

            //Act
            var result = await Service.AddGenreToFavoriteAsync(1,1);

            //Assert
            using var db = CreateDbContext();
            var actualResult = await db.Users.Include(x => x.FavoriteGenres).FirstOrDefaultAsync(u => u.Id == expectedResult.Id);

            Assert.True(result);
            Assert.NotNull(actualResult);
            VerifyUser(expectedResult, actualResult);
        }
    }
}
