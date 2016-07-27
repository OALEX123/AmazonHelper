namespace AmazonHelper.DataAccess
{
    using System.Data.Entity;
    using Database;
    using Models;
    public class AmazonHelperDb : DbContext, IAmazonHelperContext
    {
        public AmazonHelperDb()
        {
            Configuration.LazyLoadingEnabled = false;
            System.Data.Entity.Database.SetInitializer(new AmazonHelperDbInitializer());
            Database.Initialize(true);
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<CommonSettings> CommonSettings { get; set; }
        public IDbSet<ProductGroup> ProductGroups { get; set; }

        public IDbSet<StatsEntry> StatsEntries { get; set; }

        public IDbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());

            modelBuilder.Configurations.Add(new ProductMap());

            modelBuilder.Configurations.Add(new ProductGroupMap());

            modelBuilder.Configurations.Add(new CommonSettingsMap());

            modelBuilder.Configurations.Add(new StatsEntryMap());

            modelBuilder.Configurations.Add(new LogEntryMap());
        }
    }
}
