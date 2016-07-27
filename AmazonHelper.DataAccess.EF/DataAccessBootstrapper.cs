namespace AmazonHelper.DataAccess.EF
{
    using Microsoft.Practices.Unity;
    using Services;

    public static class DataAccessBootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IDbContextFactory, DbContextFactory>()
                .RegisterType<IUserDataAccess, UserDataAccess>()
                .RegisterType<IProductDataAccess, ProductDataAccess>()
                .RegisterType<ICommonSettingsDataAccess, CommonSettingsDataAccess>();
        }
    }
}
