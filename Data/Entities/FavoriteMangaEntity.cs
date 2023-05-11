namespace Data.Entities
{
    public class FavoriteMangaEntity : FavoriteItemEntity
    {
        public MangaEntity Manga { get; set; }

        public FavoriteMangaEntity()
        {
            
        }
        public FavoriteMangaEntity(UserEntity user, MangaEntity manga)
        {
            User = user;
            Manga = manga;
        }
    }
}
