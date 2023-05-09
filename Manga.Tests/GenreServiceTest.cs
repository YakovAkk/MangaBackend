using Data.Entities;
using Manga.Tests.Base;
using Services.Core.Paginated;
using Services.Model.DTO;
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
        public async void AddRangeAsyncTestRegularCase()
        {
            //Arrange
            var list = new List<GenreInput>()
            {
               new GenreInput("genreInput1"),
               new GenreInput("genreInput2"),
            };

            //Act
            var result = await Service.AddRange(list);

            //Assert
            Assert.Equal(list.Count, result.Count);

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(list[i].Name, result[i].Name);
            }
        }
        [Fact]
        public async void GetAllAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedList = new List<GenreEntity>() 
            { 
                new GenreEntity()
                {
                    Name = "genre1"
                },
                new GenreEntity()
                {
                    Name = "aaa"
                }
            };

            //Act
            var result = await Service.GetAllAsync();

            //Assert          
            Assert.Equal(expectedList.Count, result.Count);
            for (int i = 0; i < expectedList.Count; i++)
            {
                Assert.Equal(expectedList[i].Name, result[i].Name);
            }
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
                    new MangaEntity()
                    {
                        Name = "TestManga",
                        AgeRating = "18+",
                        Description = "Description",
                        Author = "Author",
                        NumbetOfChapters = 1,
                        PathToFoldersWithGlava = new List<GlavaMangaEntity> { 
                            new GlavaMangaEntity()
                            {
                                LinkToFirstPicture = "./",
                                NumberOfGlava = 2,
                                NumberOfPictures = 1,
                            }},
                        PathToTitlePicture = "./"
                    }
                }
            };

            //Act
            var result = await Service.GetByIdAsync(2);

            //Assert
            Assert.Equal(expectedResult.Name, result.Name);
            for (int i = 0; i < expectedResult.Mangas.Count; i++)
            {
                Assert.Equal(expectedResult.Mangas[i].Name, result.Mangas[i].Name);
                Assert.Equal(expectedResult.Mangas[i].AgeRating, result.Mangas[i].AgeRating);
                Assert.Equal(expectedResult.Mangas[i].Author, result.Mangas[i].Author);
                Assert.Equal(expectedResult.Mangas[i].Description, result.Mangas[i].Description);
                Assert.Equal(expectedResult.Mangas[i].PathToTitlePicture, result.Mangas[i].PathToTitlePicture);
                Assert.Equal(expectedResult.Mangas[i].ReleaseYear, result.Mangas[i].ReleaseYear);
                Assert.Equal(expectedResult.Mangas[i].NumbetOfChapters, result.Mangas[i].NumbetOfChapters);
            }
        }
        [Fact]
        public async void GetPaginatedGenreListAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var items = new List<GenreEntity>()
            {
                new GenreEntity() {Name = "genre1"},
                new GenreEntity() {Name = "aaa" }
            };

            var expectedResult = new PagedResult<List<GenreEntity>, object>(2, items, null);

            //Act
            var result = await Service.GetPaginatedGenreListAsync(2,1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Meta.TotalCount, result.Meta.TotalCount);

            for (int i = 0; i < expectedResult.Items.Count; i++)
            {
                Assert.Equal(expectedResult.Items[i].Name, result.Items[i].Name);
            }
        }
        [Fact]
        public async void FiltrationByNameAsyncTestRegularCase()
        {
            //Arrange
            SetupEnvironmentData();

            var expectedResult = new List<GenreEntity>()
            {
                new GenreEntity() { Name = "genre1" }
            };

            //Act
            var result = await Service.FiltrationByNameAsync("genre");

            //Assert
            Assert.NotNull(result);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(expectedResult[i].Name, result[i].Name);
            }
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
        public async void IsGenreExistAsyncTestRegularCase(int genreId, bool expectedResult)
        {
            //Arrange
            SetupEnvironmentData();

            //Act
            var result = await Service.IsGenreExistAsync(genreId);

            //Assert
            Assert.Equal(expectedResult,result);
        }
    }
}
