namespace AmazonHelper.DataAccess.Database
{
    using System.Data.Entity;
    using Models;

    internal class AmazonHelperDbInitializer : CreateDatabaseIfNotExists<AmazonHelperDb>
    {
        protected override void Seed(AmazonHelperDb context)
        {
            #region adding default user  

            var user = context.Users.Add(new User
            {
                UserName = "admin",
                Password = "12345",
                Email = "admin@test.com",
                IsActive = true,
                CommonSettings = new CommonSettings
                {
                    Email = "foramazonnotifications@gmail.com",
                    CompanyName = "Amazon"
                }
            });

            context.SaveChanges();

            #endregion

            #region adding default settings  

            var group1 = context.ProductGroups.Add(new ProductGroup { GroupName = "Once in 10 mins", Interval = 10 });
            var group2 = context.ProductGroups.Add(new ProductGroup { GroupName = "Once an hour", Interval = 30 });
            var group3 = context.ProductGroups.Add(new ProductGroup { GroupName = "Once a day", Interval = 90 });

            context.SaveChanges();

            for (int i = 0; i < 100; i++)
            {
                context.Products.Add(new Product { Asin = "B00NHQFA1I", Group = group1, ProductName = "B00NHQFA1I", IsActive = true, IsNotificationEnabled = true });
                context.Products.Add(new Product { Asin = "B007GE75HY", Group = group1, ProductName = "B007GE75HY", IsActive = true, IsNotificationEnabled = true });
                context.Products.Add(new Product { Asin = "B0085Y3X3Y", Group = group1, ProductName = "B0085Y3X3Y", IsActive = true, IsNotificationEnabled = true });
                context.Products.Add(new Product { Asin = "B00NW2Q6ZG", Group = group2, ProductName = "B00NW2Q6ZG", IsActive = true, IsNotificationEnabled = true });
                context.Products.Add(new Product { Asin = "B00NI36G0O", Group = group2, ProductName = "B00NI36G0O", IsActive = true, IsNotificationEnabled = true });
            }

            context.SaveChanges();

            #endregion

            base.Seed(context);
        }
    }
}



