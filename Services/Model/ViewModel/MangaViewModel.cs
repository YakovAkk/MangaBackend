using Data.Entities;

namespace Services.Model.ViewModel
{
    public class MangaViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PathToTitlePicture { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public string AgeRating { get; set; }
        public int NumbetOfChapters { get; set; }
        public string Author { get; set; }
        public virtual List<GenreEntity> Genres { get; set; }
        public virtual List<GlavaMangaEntityViewModel> PathToFoldersWithGlava { get; set; }
    }

    public class GlavaMangaEntityViewModel
    {
        public int NumberOfGlava { get; set; }
        public List<string> LinkToPictures { get; set; }
        public int NumberOfPictures { get; set; }
    }
}
