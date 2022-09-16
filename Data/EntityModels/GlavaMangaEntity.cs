using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class GlavaMangaEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public int NumberOfGlava { get; set; }
        public string LinkToFirstPicture { get; set; }
        public int AmountOfPictures { get; set; }
    }
}
