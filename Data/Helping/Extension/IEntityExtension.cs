using Data.Entities.Base;

namespace Data.Helping.Extension
{
    public static class IEntityExtension
    {
        public static IList<object> ToList(this IEntity model)
        {
            return new List<object>() { model };
        }
    }
}
