namespace AmazonHelper.Business.Services
{
    using Microsoft.Practices.Unity;
    using DataAccess;

    public static class BusinnessBootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IDbContextFactory, DbContextFactory>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ICommonSettingsService, CommonSettingsService>();
            container.RegisterType<IStatsService, StatsService>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<ILogService, LogService>();
        }
    }
}
