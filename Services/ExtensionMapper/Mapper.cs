using Data.Models;
using Services.DTO;

namespace Services.ExtensionMapper
{
    public static class Mapper
    {
        public static GenreEntity toEntity(this GenreDTO dto)
        {
            return new GenreEntity()
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public static MangaEntity toEntity(this MangaDTO dto, List<GenreEntity> genres)
        {
            return new MangaEntity()
            {
                Id = dto.Id,
                Name = dto.Name,
                PathToTitlePicture = dto.PathToTitlePicture,
                Description = dto.Description,
                PathToFoldersWithGlava = dto.PathToFoldersWithGlava,
                AgeRating = dto.AgeRating,
                Author = dto.Author,
                IsFavorite = dto.IsFavorite,
                NumbetOfChapters = dto.NumbetOfChapters,
                ReleaseYear = dto.ReleaseYear,
                Genres = genres
            };
        }
    }
}
