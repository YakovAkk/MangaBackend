using Data.Entities.Base;

namespace Data.Helping.Extension
{
    public static class IModelExtension
    {
        public static IList<object> ToList(this IEntity model)
        {
            return new List<object>() { model };
        }
    }
}
