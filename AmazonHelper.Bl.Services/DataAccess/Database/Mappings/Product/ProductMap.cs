namespace AmazonHelper.DataAccess.Database
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

            Property(t => t.GroupId)
                .IsRequired();

            Property(t => t.IsActive)
               .IsRequired();

            Property(t => t.IsNotificationEnabled)
               .IsRequired();

            Property(t => t.ProductName)
                .HasMaxLength(DatabaseConsts.DefaultStringLength);

            Property(t => t.Asin)
              .HasMaxLength(DatabaseConsts.DefaultStringLength);

            HasRequired(t => t.Group)
                .WithMany(t => t.Products)
                .HasForeignKey(t => t.GroupId);

            HasMany(t => t.StatsEntries)
                .WithRequired(t => t.Product)
                .HasForeignKey(t => t.ProductId);

            // Table & Column Mappings
            ToTable("Products", "dbo");
        }
    }
}
