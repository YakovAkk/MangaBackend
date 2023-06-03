using Data.Entities;
using Services.Model.DTO;
using Services.Model.InputModel;
using Services.Model.ViewModel;

namespace Services.Extensions.ExtensionMapper;

public static class Mapper
{

    #region MangaEntity
    public static MangaEntity toEntity(this MangaInput dto, List<GenreEntity> genres)
    {
        return new MangaEntity()
        {
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
