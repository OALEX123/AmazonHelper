[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AmazonHelper.WebApp.App_Start.Combres), "PreStart")]
namespace AmazonHelper.WebApp.App_Start {
	using System.Web.Routing;
	using global::Combres;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}