namespace Services.Core
{
    public class ContextPool<T> : IDisposable where T : IDisposable
    {
        private readonly Func<T> createContext;
        private readonly List<T> dbContexts;

        public ContextPool(Func<T> createContext)
        {
            this.createContext = createContext;
            dbContexts = new List<T>();
        }

        public void Dispose()
        {
            foreach (var dbContext in dbContexts)
                dbContext.Dispose();
        }

        public T NewContext()
        {
            var dbContext = createContext();
            dbContexts.Add(dbContext);
            return dbContext;
        }
    }
}
