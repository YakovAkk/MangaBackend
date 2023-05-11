using Data.Entities.Base;

namespace Data.Entities
{
    public abstract class FavoriteItemEntity : IEntity
    {
        public int Id { get; set; }
        public UserEntity User { get; set; }
    }
}
