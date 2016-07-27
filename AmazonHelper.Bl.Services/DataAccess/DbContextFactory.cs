namespace AmazonHelper.DataAccess
{
    using Database;

    internal class DbContextFactory: IDbContextFactory
    {
        public IAmazonHelperContext GetAmazonHelperContext()
        {
            return new AmazonHelperDb();
        }
    }
}
