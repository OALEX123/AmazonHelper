namespace AmazonHelper.DataAccess.Database
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Models;
    internal class CommonSettingsMap : EntityTypeConfiguration<CommonSettings>
    {
        public CommonSettingsMap()
        {
            HasKey(t => t.UserId);

            Property(t => t.Email)
               .IsOptional()
              .HasMaxLength(DatabaseConsts.DefaultStringLength);

            Property(t => t.Email)
              .IsOptional()
              .HasMaxLength(DatabaseConsts.DefaultStringLength);

            Property(t => t.CompanyName)
                .IsOptional()
                .HasMaxLength(DatabaseConsts.DefaultStringLength);

            HasRequired(t => t.User)
                .WithOptional(t => t.CommonSettings);

            // Table & Column Mappings
            ToTable("CommonSettings", "dbo");
        }
    }
}
