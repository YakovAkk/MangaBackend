using Data.Entities;
using Manga.Tests.Base;
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
        public async void GetByIdAsyncRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var expectedManga = new MangaEntity()
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

            //Act
            var result = await Service.GetByIdAsync(1);

            //Assert
            
            Assert.NotNull(expectedManga);
            Assert.Equal(expectedManga.Name, result.Name);
            Assert.Equal(expectedManga.Author, result.Author);
            Assert.Equal(expectedManga.AgeRating, result.AgeRating);
            Assert.Equal(expectedManga.Description, result.Description);
            Assert.Equal(expectedManga.NumbetOfChapters, result.NumbetOfChapters);
            Assert.Equal(expectedManga.PathToTitlePicture, result.PathToTitlePicture);
        }
    }
}
