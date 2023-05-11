using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class GlavaMangaEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    public int NumberOfGlava { get; set; }
    public string LinkToFirstPicture { get; set; }
    public int NumberOfPictures { get; set; }
}

