using Data.Database;
using Data.Entities;
using Manga.Tests.Base;
using Microsoft.EntityFrameworkCore;
using Services.Services;
using Xunit;

namespace Manga.Tests
{
    public class GenreServiceTest : BaseTest
    {
        private GenreService Service;

        public GenreServiceTest()
        {
            Service = new GenreService(options);
        }

        [Fact]
        public async void CreateUserTestRegularCase()
        {
            //Arrange
            using var db = CreateDbContext();

            var genre = new GenreEntity()
            {
                Name = "TestGenre"
            };

            db.Genres.Add(genre);

            await db.SaveChangesAsync();

            //Act
            var result = await Service.GetByIdAsync(genre.Id);

            //Assert
            
            var expectedGenre = await db.Genres.FirstOrDefaultAsync(u => u.Id == result.Id);

            Assert.NotNull(expectedGenre);
            Assert.Equal(expectedGenre.Name, result.Name);
        }
    }
}
