using Data.Entities;
using Manga.Tests.Tests.Base;
using Manga.Tests.Utility;
using Services;
using Services.Core.Paginated;
using Services.Model.InputModel;
using Services.Services;
using Xunit;

namespace Manga.Tests.Tests
{
    public class GenreServiceTest : BaseTest
    {
        private GenreService Service;

        public GenreServiceTest()
        {
            Service = new GenreService(options);
        }

        [Fact]
        public async void AddRangeAsyncTestRegularCase()
        {
            //Arrange
            var genreInput = new List<GenreInputModel>()
            {
               new GenreInputModel("genreInput1"),
               new GenreInputModel("genreInput2"),
            };
            var expectedResult = genreInput.Select(x => x.MapTo<GenreEntity>()).ToList();

            //Act
            var actualResult = await Service.AddRangeAsync(genreInput);

            //Assert
            Assert.Equal(expectedResult.Count, actualResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
                VerifyGenre(expectedResult[i], actualResult[i]);
        }
        [Fact]
        public async void GetAllAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var expectedResult = new List<GenreEntity>()
            {
                 new GenreEntity()
                {
                    Name = "aaa"
                },
                Util.GetGenre()
               
            };

            //Act
            var actualResult = await Service.GetAllAsync();

            //Assert          
            Assert.Equal(expectedResult.Count, actualResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
                VerifyGenre(expectedResult[i], actualResult[i]);
        }
        [Fact]
        public async void GetByIdAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var expectedResult = new GenreEntity()
            {
                Name = "aaa",
                Mangas = new List<MangaEntity>()
                {
                    Util.GetManga()
                }
            };

            //Act
            var actualResult = await Service.GetByIdAsync(2);

            //Assert
            VerifyGenre(expectedResult, actualResult);
        }
        [Fact]
        public async void GetPaginatedGenreListAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var items = new List<GenreEntity>()
            {
                Util.GetGenre(),
                new GenreEntity() {Name = "aaa" }
            };
            var expectedResult = new PagedResult<List<GenreEntity>, object>(2, items, null);

            //Act
            var actualResult = await Service.GetPaginatedGenreListAsync(2, 1);

            //Assert
            Assert.Equal(expectedResult.Meta.TotalCount, actualResult.Meta.TotalCount);
            for (int i = 0; i < expectedResult.Items.Count; i++)
                VerifyGenre(expectedResult.Items[i], actualResult.Items[i]);
        }
        [Fact]
        public async void FiltrationByNameAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();
            var expectedResult = new List<GenreEntity>()
            {
                Util.GetGenre()
            };

            //Act
            var actualResult = await Service.FiltrationByNameAsync("genre");

            //Assert
            Assert.NotNull(actualResult);
            for (int i = 0; i < expectedResult.Count; i++)
                VerifyGenre(expectedResult[i], actualResult[i]);
        }

        public static IEnumerable<object[]> DataForGenreExistMethod =>
            new List<object[]>
            {
                new object[]
                {
                    genreId,
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
        public async void IsGenreExistAsyncTestRegularCase(int genreId, bool expectedResult)
        {
            //Arrange
            SetupEnvironmentData();

            //Act
            var result = await Service.IsGenreExistAsync(genreId);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
