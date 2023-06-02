using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Data.Entities.Base;

namespace Data.Entities;
public partial class UserEntity : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string DeviceToken { get; set; }

    // Refresh token (to refresh access token)
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? TokenCreated { get; set; }
    public DateTime? TokenExpires { get; set; }

    // Email verification token
    public string? VerificationToken { get; set; } = string.Empty;
    public DateTime? VerifiedAt { get; set; }

    // Reset password token
    public string? ResetPasswordToken { get; set; } = string.Empty;
    public DateTime? ResetPasswordTokenExpires { get; set; }

    public UserEntity()
    {
        FavoriteMangasItems = new List<FavoriteMangaEntity>();
        FavoriteGenresItems = new List<FavoriteGenreEntity>();
    }

    // Navigation fields
    public virtual List<FavoriteMangaEntity> FavoriteMangasItems { get; set; }
    public virtual List<FavoriteGenreEntity> FavoriteGenresItems { get; set; }
    public virtual List<RememberReadingItem> RememberReadingItems { get; set;}
}
