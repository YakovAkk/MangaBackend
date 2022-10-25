using Data.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class GenreEntity : IModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public bool IsFavorite { get; set; }
    public virtual List<MangaEntity> Mangas { get; set; }

    public GenreEntity()
    {
        IsFavorite = false;
        Mangas = new List<MangaEntity>();
    }
}
