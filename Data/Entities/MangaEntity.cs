using Data.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class MangaEntity : IModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public string PathToTitlePicture { get; set; }
    public string Description { get; set; }
    public int ReleaseYear { get; set; }
    public string AgeRating { get; set; }
    public int NumbetOfChapters { get; set; }
    public string Author { get; set; }
    public bool IsFavorite { get; set; }
    public virtual List<GenreEntity> Genres { get; set; }
    public virtual List<GlavaMangaEntity> PathToFoldersWithGlava { get; set; }
    public MangaEntity()
    {
        PathToFoldersWithGlava = new List<GlavaMangaEntity>();
        IsFavorite = false;
    }
}

