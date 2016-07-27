namespace AmazonHelper.DataAccess
{
    public interface IDbContextFactory
    {
        IAmazonHelperContext GetAmazonHelperContext();
    }
}
