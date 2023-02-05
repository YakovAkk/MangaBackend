using Data.Entities;
using Services.Model.DTO;
using Services.Model.ViewModel;

namespace Services.ExtensionMapper;

public static class Mapper
{
    #region GenreEntity
    public static GenreEntity toEntity(this GenreDTO dto)
    {
        return new GenreEntity()
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }
    #endregion  

    #region MangaEntity
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
            NumbetOfChapters = dto.NumbetOfChapters,
            ReleaseYear = dto.ReleaseYear,
            Genres = genres
        };
    }
    #endregion

    #region UserEntity
    public static UserEntity toEntity(this UserRegistrationDTO dto)
    {
        return new UserEntity()
        {
            DeviceToken = dto.DeviceToken,
            Name = dto.UserName,
            Password = dto.Password,
            Email = dto.Email,
        };
    }

    public static UserViewModel toViewModel(this UserEntity user)
    {
        return new UserViewModel()
        {
            Id = user.Id,
            Name = user.Name,
            DeviceToken = user.DeviceToken,
            Email = user.Email,
            FavoriteGenres = user.FavoriteGenres,
            FavoriteMangas = user.FavoriteMangas,
        };
    }
    #endregion
}
