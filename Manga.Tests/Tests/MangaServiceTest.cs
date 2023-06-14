using Data.Entities;
using Manga.Tests.Tests.Base;
using Manga.Tests.Utility;
using Moq;
using Services.Core.Paginated;
using Services.Services;
using Services.Services.Base;
using Services.Shared.Configuration;
using Xunit;

namespace Manga.Tests.Tests
{
    public class MangaServiceTest : BaseTest
    {
        private MangaService Service;
        private Mock<IGenreService> _genreServiceMock;

        public MangaServiceTest()
        {
            var conf = new OthersConfiguration();
            _genreServiceMock = new Mock<IGenreService>();
            Service = new MangaService(conf, options, _genreServiceMock.Object);
        }
      
        [Fact]
        public async void GetByIdAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var expectedResult = Util.GetManga();

            //Act
            var actualResult = await Service.GetByIdAsync(1);

            //Assert
            Assert.NotNull(expectedResult);
            VerifyMangaEntity(expectedResult, actualResult);
        }
        [Fact]
        public async void GetPagiantedMangaListAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var items = new List<MangaEntity>()
            {
                Util.GetManga()
            };
            var expectedResult = new PagedResult<List<MangaEntity>, object>(1, items, null);

            //Act
            var actualResult = await Service.GetPagiantedMangaListAsync(1, 1);

            //Assert
            Assert.Equal(expectedResult.Meta.TotalCount, actualResult.Meta.TotalCount);
            for (int i = 0; i < expectedResult.Items.Count; i++)
                VerifyMangaEntity(expectedResult.Items[i], actualResult.Items[i]);
        }
        [Fact]
        public async void FiltrationByDateAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var expectedResult = new List<MangaEntity>()
            {
                Util.GetManga()
            };

            //Act
            var actualResult = await Service.FiltrationByDateAsync(2001);

            //Assert
            Assert.NotNull(actualResult);
            for (int i = 0; i < expectedResult.Count; i++)
                VerifyMangaEntity(expectedResult[i], actualResult[i]);
        }
        [Fact]
        public async void FiltrationByNameAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var expectedResult = new List<MangaEntity>()
            {
                Util.GetManga()
            };

            //Act
            var actualResult = await Service.FiltrationByNameAsync("TestManga");

            //Assert
            Assert.NotNull(actualResult);
            for (int i = 0; i < expectedResult.Count; i++)
                VerifyMangaEntity(expectedResult[i], actualResult[i]);
        }

        public static IEnumerable<object[]> DataForGenreExistMethod =>
           new List<object[]>
           {
                new object[]
                {
                    1,
                    true
                },
                new object[]
                {
                    3,
                    false
                },
           };
        [Theory]
        [MemberData(nameof(DataForGenreExistMethod))]
        public async void IsGenreExistAsyncTestRegularCase(int mangaId, bool expectedResult)
        {
            //Arrange
            SetupEnvironmentData();

            //Act
            var result = await Service.IsMangaExistAsync(mangaId);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
