namespace AmazonHelper.DataAccess.Database
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Models;
    internal class LogEntryMap : EntityTypeConfiguration<LogEntry>
    {
        public LogEntryMap()
        {
            HasKey(t => t.LogEntryId);

            // Properties
            Property(t => t.LogEntryId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Message)
                .IsRequired();

            Property(t => t.DateCreated)
                .IsRequired();

            // Table & Column Mappings
            ToTable("LogEntries", "dbo");
        }
    }
}
