namespace AmazonHelper.DataAccess.EF.Database
{
    using System.Data.Entity;
    using Models;

    internal class AmazonHelperDbInitializer : CreateDatabaseIfNotExists<AmazonHelperDb>
    {
        protected override void Seed(AmazonHelperDb context)
        {
            #region adding default user  

            context.Users.Add(new User
            {
                UserName = "admin",
                Password = "12345",
                Email = "admin@test.com",
                IsActive = true
            });

            context.SaveChanges();

            #endregion

            #region adding default settings  

            context.CommonSettings.Add(new CommonSettings
            {
                UserId = 1,
                Email = "amazon@test.com",
                CompanyName = "Amazon"
            });

            context.SaveChanges();

            #endregion

            base.Seed(context);
        }
    }
}



