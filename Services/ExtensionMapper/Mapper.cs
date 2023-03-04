using Data.Entities;
using Services.Model.DTO;
using Services.Model.ViewModel;

namespace Services.ExtensionMapper;

public static class Mapper
{
    #region GenreEntity
    public static GenreEntity toEntity(this GenreInput dto)
    {
        return new GenreEntity()
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }
    #endregion  

    #region MangaEntity
    public static MangaEntity toEntity(this MangaInput dto, List<GenreEntity> genres)
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
    public static UserEntity toEntity(this UserRegistrationDTO dto, byte[] passwordHash, byte[] passwordSalt, string verificationToken)
    {
        return new UserEntity()
        {
            DeviceToken = dto.DeviceToken,
            Name = dto.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Email = dto.Email,
            VerificationToken = verificationToken 
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
