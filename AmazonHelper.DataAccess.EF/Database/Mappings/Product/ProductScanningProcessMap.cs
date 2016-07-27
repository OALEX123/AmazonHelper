namespace AmazonHelper.DataAccess.EF.Database
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Models;
    internal class ProductScanningProcessMap : EntityTypeConfiguration<ProductScanningProcess>
    {
        public ProductScanningProcessMap()
        {
            HasKey(t => t.ProductScanningProcessId);

            // Properties
            Property(t => t.ProductScanningProcessId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.ProductId)
                 .IsRequired();

            Property(t => t.ScanningEvent)
                .IsRequired();

            Property(t => t.ScanningGroup)
                 .IsRequired();

            Property(t => t.IsActive)
                 .IsRequired();

            HasRequired(t => t.Product)
                .WithMany(t => t.ScanningProcesses)
                .HasForeignKey(t => t.ProductId);

            // Table & Column Mappings
            ToTable("ProductScanningProcesses", "dbo");
        }
    }
}
