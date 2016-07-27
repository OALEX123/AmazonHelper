namespace AmazonHelper.DataAccess.Database
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Models;
    internal class StatsEntryMap : EntityTypeConfiguration<StatsEntry>
    {
        public StatsEntryMap()
        {
            HasKey(t => t.StatsEntryId);

            // Properties
            Property(t => t.StatsEntryId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.ProductId)
                .IsRequired();

            Property(t => t.GroupId)
                .IsRequired();

            Property(t => t.DateCreated)
                .IsRequired();

            Property(t => t.Result)
                .IsRequired();

            HasRequired(t => t.Product)
                .WithMany(t => t.StatsEntries)
                .HasForeignKey(t => t.ProductId);

            // Table & Column Mappings
            ToTable("StatsEntries", "dbo");
        }
    }
}
