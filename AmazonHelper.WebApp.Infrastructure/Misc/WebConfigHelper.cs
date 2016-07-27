namespace AmazonHelper.WebApp.Infrastructure
{
    using System;

    public class WebConfigHelper
    {
        private const string WebApiUrlKey = "WebApiUrl";
        public static string GetWebApiUrl()
        {
            return RetriveKey(WebApiUrlKey);
        }

        private static readonly Func<string, string> RetriveKey = key => System.Configuration.ConfigurationManager.AppSettings[key];
    }
}
