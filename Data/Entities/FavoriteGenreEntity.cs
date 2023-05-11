namespace Data.Entities
{
    public class FavoriteGenreEntity : FavoriteItemEntity
    {
        public GenreEntity Genre { get; set; }

        public FavoriteGenreEntity()
        {
            
        }

        public FavoriteGenreEntity(UserEntity user, GenreEntity genre)
        {
            Genre = genre;
            User = user;
        }
    }
}
