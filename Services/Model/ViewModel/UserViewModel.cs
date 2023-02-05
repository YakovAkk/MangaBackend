using Data.Entities;

namespace Services.Model.ViewModel
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DeviceToken { get; set; }
        public virtual List<MangaEntity> FavoriteMangas { get; set; }
        public virtual List<GenreEntity> FavoriteGenres { get; set; }
    }
}
