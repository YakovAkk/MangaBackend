using Data.Entities;
using Manga.Tests.Tests.Base;
using Manga.Tests.Utility;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Core.Paginated;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Services;
using Services.Storage.Base;
using Xunit;

namespace Manga.Tests.Tests
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
        public async void GetAllAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var expectedResult = new List<MangaEntity>()
            {
                Util.GetManga()
            };

            //Act
            var actualResult = await Service.GetAllAsync();

            //Assert          
            Assert.Equal(expectedResult.Count, actualResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
                VerifyManga(expectedResult[i], actualResult[i]);
        }
        [Fact]
        public async void AddRangeAsyncTestRegularCase()
        {
            //Arrange
            var mangaInput = new List<MangaInput>()
            {
                new MangaInput()
                {
                    Name = "Attack of the Titans",
                    PathToTitlePicture = "manga/attackofthetitans/titleimage.jpg",
                    Genres_Ids = new List<int>() { genreId },
                    PathToFoldersWithGlava = new List<GlavaMangaEntity>()
                    {
                         new GlavaMangaEntity()
                         {
                            NumberOfGlava = 4,
                            LinkToFirstPicture = "manga/attackofthetitans/glava4/1.jpg",
                            NumberOfPictures = 49
                         }
                    },
                    Description = "Давным-давно человечество было всего лишь «их» кормом, до тех пор, пока оно не построило гигантскую стену вокруг своей страны. С тех пор прошло сто лет мира и большинство людей жили счастливой, беззаботной жизнью. Но за долгие годы спокойствия пришлось заплатить огромную цену, и в 845 году они снова познали чувство ужаса и беспомощности – стена, которая была их единственным спасением, пала. «Они» прорвались. Половина человечества съедена, треть территории навсегда потеряна...",
                    NumbetOfChapters = 140,
                    AgeRating = "18+",
                    Author = "ISAYAMA Hajime",
                    ReleaseYear = 2009
                }
            };

            using var db = CreateDbContext();
            var genres = await db.Genres.Where(x => x.Id == genreId).ToListAsync();
            var expectedResult = mangaInput.Select(x => x.toEntity(genres)).ToList();

            //Act
            var actualResult = await Service.AddRangeAsync(mangaInput);

            //Assert
            for (int i = 0; i < expectedResult.Count; i++)
            {
                VerifyManga(expectedResult[i], actualResult[i]);
            }
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
            VerifyManga(expectedResult, actualResult);
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
                VerifyManga(expectedResult.Items[i], actualResult.Items[i]);
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
                VerifyManga(expectedResult[i], actualResult[i]);
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
                VerifyManga(expectedResult[i], actualResult[i]);
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
