namespace Services.Core.Paginated
{
    public class PagedResult<TResult, TFilter>
    {
        public TResult Items { get; set; }
        public MetaData<TFilter> Meta { get; set; }

        public PagedResult(int totalCount, TResult items, TFilter filters)
        {
            Items = items;
            Meta = new MetaData<TFilter>(totalCount, filters);
        }
    }

    public class MetaData<TFilter>
    {
        public int TotalCount { get; set; }
        public TFilter Filters { get; set; }
        public MetaData(int totalCount, TFilter filters)
        {
            TotalCount = totalCount;
            Filters = filters;
        }
    }
}
