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

            await dbInit.SaveChangesAsync();
        }
    }
}
