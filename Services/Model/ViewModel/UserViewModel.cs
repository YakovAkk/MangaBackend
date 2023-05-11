using Data.Entities;

namespace Services.Model.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DeviceToken { get; set; }
        public List<GenreEntity> FavoriteGenres { get; set;}
        public List<MangaEntity> FavoriteMangas { get; set; }
    }
}
