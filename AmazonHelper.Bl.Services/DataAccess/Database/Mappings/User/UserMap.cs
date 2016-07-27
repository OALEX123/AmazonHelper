namespace AmazonHelper.DataAccess.Database
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Models;
    internal class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            HasKey(t => t.UserId);

            // Properties
            Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.UserName)
                .HasMaxLength(DatabaseConsts.DefaultStringLength);

            Property(t => t.Email)
              .HasMaxLength(DatabaseConsts.DefaultStringLength);

            Property(t => t.Password)
                .HasMaxLength(DatabaseConsts.DefaultStringLength);

            HasOptional(t => t.CommonSettings)
                .WithRequired(t => t.User);

            // Table & Column Mappings
            ToTable("Users", "dbo");
        }
    }
}
