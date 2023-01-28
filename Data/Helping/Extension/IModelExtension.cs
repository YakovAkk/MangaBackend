using Data.Entities.Base;

namespace Data.Helping.Extension
{
    public static class IModelExtension
    {
        public static IList<object> ToList(this IModel model)
        {
            return new List<object>() { model};
        }
    }
}
