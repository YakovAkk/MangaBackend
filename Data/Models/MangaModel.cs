using Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class MangaModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public string PathToTitlePicture { get; set; } 
        public string Description { get; set; }
        public string MessageWhatWrong { get; set; }
        public int ReleaseYear { get; set; }
        public string AgeRating { get; set; }
        public int NumbetOfChapters { get; set; }
        public string Author { get; set; }
        public bool IsFavorite { get; set; }
        public virtual List<GenreModel> Genres { get; set; }
        public virtual List<GlavaMangaModel> PathToFoldersWithGlava { get; set; }
        public MangaModel()
        {
            PathToFoldersWithGlava = new List<GlavaMangaModel>();
            IsFavorite = false;
        }
    }
}
