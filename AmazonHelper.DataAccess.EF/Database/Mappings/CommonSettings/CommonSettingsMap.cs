namespace AmazonHelper.DataAccess.EF.Database
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Models;
    internal class CommonSettingsMap : EntityTypeConfiguration<CommonSettings>
    {
        public CommonSettingsMap()
        {
            HasKey(t => t.CommonSettingsId);

            // Properties
            Property(t => t.CommonSettingsId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.UserId)
                .IsRequired();

            Property(t => t.Email)
               .IsOptional()
              .HasMaxLength(DatabaseConsts.DefaultStringLength);

            Property(t => t.Email)
              .IsOptional()
              .HasMaxLength(DatabaseConsts.DefaultStringLength);

            Property(t => t.CompanyName)
                .IsOptional()
                .HasMaxLength(DatabaseConsts.DefaultStringLength);

            //HasRequired(t => t.User)
            //    .WithOptional(t => t.CommonSettings);

            // Table & Column Mappings
            ToTable("CommonSettings", "dbo");
        }
    }
}
