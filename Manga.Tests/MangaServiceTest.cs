using Data.Entities;
using Manga.Tests.Base;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Services;
using Services.Storage.Base;
using Xunit;

namespace Manga.Tests
{
    public class MangaServiceTest : BaseTest
    {
        private MangaService Service;
        private Mock<ILocalStorage> _localServiceMock;

        public MangaServiceTest()
        {
            _localServiceMock = new Mock<ILocalStorage>();

            Service = new MangaService(_localServiceMock.Object, options);
        }

        [Fact]
        public async void CreateMangaRegularCase()
        {
            //Arrange
            using var db = CreateDbContext();

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

            db.Mangas.Add(manga);

            await db.SaveChangesAsync();

            //Act
            var result = await Service.GetByIdAsync(manga.Id);

            //Assert
            var expectedGenre = await db.Mangas.FirstOrDefaultAsync(u => u.Id == result.Id);

            Assert.NotNull(expectedGenre);
            Assert.Equal(expectedGenre.Name, result.Name);
            Assert.Equal(expectedGenre.Author, result.Author);
            Assert.Equal(expectedGenre.AgeRating, result.AgeRating);
            Assert.Equal(expectedGenre.Description, result.Description);
            Assert.Equal(expectedGenre.NumbetOfChapters, result.NumbetOfChapters);
            Assert.Equal(expectedGenre.PathToTitlePicture, result.PathToTitlePicture);
        }
    }
}
