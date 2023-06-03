using Data.Entities;

namespace Services.Model.ViewModel
{
    public class RememberReadingItemViewModel
    {
        public int Id { get; set; }
        public UserViewModel User { get; set; }
        public MangaEntity Manga { get; set; }
        public int ChapterNumber { get; set; }
        public int Page { get; set; }
    }
}
