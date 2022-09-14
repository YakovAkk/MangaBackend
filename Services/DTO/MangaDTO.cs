using Data.Models;
using Services.DTO.Base;

namespace Services.DTO
{
    public class MangaDTO : IModelDTO
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string PathToTitlePicture { get; set; }
        public int ReleaseYear { get; set; }
        public string AgeRating { get; set; }
        public int NumbetOfChapters { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
    
        public List<GlavaMangaModel> PathToFoldersWithGlava { get; set; }
        public List<string> genres_id { get; set; }

        public MangaDTO()
        {
            IsFavorite = false;
            PathToFoldersWithGlava = new List<GlavaMangaModel>();
            genres_id = new List<string>();
        }

    }
}
