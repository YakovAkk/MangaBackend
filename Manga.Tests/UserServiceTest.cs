using Data.Entities;
using Manga.Tests.Base;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Model.InputModel;
using Services.Services;
using Services.Services.Base;
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
            var user = new UserEntity()
            {
                Email = "User@gmail.com",
                Name = "TestUser",
                DeviceToken = "test device token",
                PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
                PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
            };

            //Act
            var result = await Service.CreateAsync(user);

            //Assert
            using var db = CreateDbContext();
            var expectedUser = await db.Users.FirstOrDefaultAsync(u => u.Id == result.Id);

            Assert.NotNull(expectedUser);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.Email, result.Email);
        }
        [Fact]
        public async void UpdateUserTestChangeNameCase()
        {
            //Arrange
            SetupEnvironmentData();
            UserInputModel userToUpdate;

            userToUpdate = new UserInputModel()
            {
                Id = 1,
                Name = "UpdatedName"
            };

            //Act
            var result = await Service.UpdateUserAsync(userToUpdate);

            //Assert
            using var db = CreateDbContext();

            var expectedUser = await db.Users.FirstOrDefaultAsync(u => u.Id == userToUpdate.Id);

            Assert.True(result);
            Assert.NotNull(expectedUser);
            Assert.Equal(expectedUser.Name, userToUpdate.Name);
        }
        [Fact]
        public async void IsUserExistsAsyncTestChangeNameCase()
        {
            //Arrange
            SetupEnvironmentData();

            //Act
            var actualResult = await Service.IsUserExistsAsync("User@gmail.com", "TestUser");

            //Assert
            Assert.True(actualResult);
        }
    }
}
