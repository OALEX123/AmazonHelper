namespace AmazonHelper.DataAccess
{
    using System.Data.Entity;
    using Models;

    public interface IAmazonHelperContext : IDbContext
    {
         IDbSet<User> Users { get; set; }

        IDbSet<Product> Products { get; set; }

        IDbSet<CommonSettings> CommonSettings { get; set; }

        IDbSet<ProductGroup> ProductGroups { get; set; }
        IDbSet<StatsEntry> StatsEntries { get; set; }

        IDbSet<LogEntry> LogEntries { get; set; }
    }
}
