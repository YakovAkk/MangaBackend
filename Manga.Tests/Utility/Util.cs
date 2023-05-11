using Data.Entities;

namespace Manga.Tests.Utility
{
    public class Util
    {
        public static UserEntity GetUser() => new UserEntity()
        {
            Id = 1,
            Email = "User@gmail.com",
            Name = "TestUser",
            DeviceToken = "test device token",
            PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, },
            PasswordSalt = new byte[] { 1, 1, 2, 45, 6 }
        };
        public static MangaEntity GetManga() => new MangaEntity()
        {
            Name = "TestManga",
            AgeRating = "18+",
            Description = "Description",
            Author = "Author",
            NumbetOfChapters = 1,
            ReleaseYear = 2000,
            PathToFoldersWithGlava = new List<GlavaMangaEntity> { new GlavaMangaEntity()
                {
                    LinkToFirstPicture = "./",
                    NumberOfGlava = 2,
                    NumberOfPictures = 1,
                }},
            PathToTitlePicture = "./"
        };
        public static GenreEntity GetGenre() => new GenreEntity()
        {
            Name = "genre1",            
        };
    }
}
