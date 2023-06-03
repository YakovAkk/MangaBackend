using Data.Entities;
using Services.Model.DTO;
using Services.Model.InputModel;
using Services.Model.ViewModel;

namespace Services.Extensions.ExtensionMapper;

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
            Name = dto.Name,
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
            Email = user.Email
        };
    }
    #endregion

    #region Remember reading
    public static RememberReadingItem toEntity(this RememberReadingItemInputModel inputModel, UserEntity user, MangaEntity manga)
    {
        return new RememberReadingItem()
        {
            User = user,
            MangaId = manga.Id,
            ChapterNumber = inputModel.ChapterNumner,
            Page = inputModel.Page
        };
    }

    public static RememberReadingItemViewModel toViewModel(this RememberReadingItem entity, UserViewModel user, MangaEntity manga)
    {
        return new RememberReadingItemViewModel()
        {
            User = user,
            Manga = manga,
            ChapterNumber = entity.ChapterNumber,
            Page = entity.Page
        };
    }
    #endregion
}
