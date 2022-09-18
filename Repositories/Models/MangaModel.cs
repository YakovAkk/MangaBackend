using Data.Models;
using Repositories.Models.Base;

namespace Repositories.Models
{
    public class MangaModel : IModel
    {
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
        public string MessageWhatWrong { get; set; }

        public MangaModel()
        {
            Genres = new List<GenreEntity>();
            PathToFoldersWithGlava = new List<GlavaMangaEntity>();
            IsFavorite = false;
        }
    }
}
