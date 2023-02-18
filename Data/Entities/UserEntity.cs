using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Data.Entities.Base;

namespace Data.Entities;
public class UserEntity : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string DeviceToken { get; set; }
    public virtual List<MangaEntity> FavoriteMangas { get; set; }
    public virtual List<GenreEntity> FavoriteGenres { get; set; }

    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? TokenCreated { get; set; }
    public DateTime? TokenExpires { get; set; }

    public string? VerificationToken { get; set; } = string.Empty;
    public DateTime? VerifiedAt { get; set; }

    public UserEntity()
    {
        FavoriteMangas = new List<MangaEntity>();
        FavoriteGenres= new List<GenreEntity>();
    }

    public override string ToString()
    {
        return $"Id = {Id} Name = {Name} DeviceToken = {DeviceToken}";
    }
}
