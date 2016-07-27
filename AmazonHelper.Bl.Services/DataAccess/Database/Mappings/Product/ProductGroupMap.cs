namespace AmazonHelper.DataAccess.Database
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Models;
    internal class ProductGroupMap : EntityTypeConfiguration<ProductGroup>
    {
        public ProductGroupMap()
        {
            HasKey(t => t.GroupId);

            // Properties
            Property(t => t.GroupId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.GroupName)
                .HasMaxLength(DatabaseConsts.DefaultStringLength);

            Property(t => t.Interval)
                .IsRequired();

            HasMany(t => t.Products)
                .WithRequired(t => t.Group)
                .HasForeignKey(t => t.GroupId);

            //HasMany(t => t.StatsEntries)
            //    .WithRequired(t => t.Group)
            //    .HasForeignKey(t => t.GroupId);

            // Table & Column Mappings
            ToTable("ProductGroups", "dbo");
        }
    }
}
