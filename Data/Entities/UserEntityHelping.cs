using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public partial class UserEntity
    {
        [NotMapped]
        public List<GenreEntity> FavoriteGenres {
            get 
            {
                return FavoriteGenresItems
                    .Select(x => x.Genre)
                    .ToList();
            } 
        }
        [NotMapped]
        public List<MangaEntity> FavoriteMangas { 
            get 
            {
                return FavoriteMangasItems
                        .Select(x => x.Manga)
                        .ToList();
            } 
        }
    }
}
