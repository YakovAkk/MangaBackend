namespace Data.Entities
{
    public abstract class FavoriteItemEntity
    {
        public int Id { get; set; }
        public UserEntity User { get; set; }
    }
}
