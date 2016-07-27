namespace AmazonHelper.DataAccess.EF.Database
{
    using System.Data.Entity;
    using Models;
    
    public class AmazonHelperDb : DbContext, IAmazonHelperContext
    {
        public AmazonHelperDb()
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new AmazonHelperDbInitializer());
            Database.Initialize(true);
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<CommonSettings> CommonSettings { get; set; }

        public IDbSet<ProductScanningProcess> ProductScanningProcesses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());

            modelBuilder.Configurations.Add(new ProductMap());

            modelBuilder.Configurations.Add(new CommonSettingsMap());
        }
    }
}
