using AmazonHelper.WebApp.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AmazonHelper.WebApp.Startup))]
namespace AmazonHelper.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            WebAppMapper.RegisterMappings();
        }
    }
}
