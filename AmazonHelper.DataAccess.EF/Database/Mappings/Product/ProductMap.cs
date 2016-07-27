namespace AmazonHelper.DataAccess.EF.Database
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Models;
    internal class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            HasKey(t => t.ProductId);

            // Properties
            Property(t => t.ProductId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.ProductName)
                .HasMaxLength(DatabaseConsts.DefaultStringLength);

            Property(t => t.Asin)
              .HasMaxLength(DatabaseConsts.DefaultStringLength);

            HasMany(t => t.ScanningProcesses)
                .WithRequired(t => t.Product)
                .HasForeignKey(t => t.ProductId);

            // Table & Column Mappings
            ToTable("Products", "dbo");
        }
    }
}
