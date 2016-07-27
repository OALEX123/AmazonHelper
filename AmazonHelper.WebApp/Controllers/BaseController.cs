namespace AmazonHelper.WebApp.Controllers
{
    using System.Text;
    using System.Web.Mvc;
    using Infrastructure;
    using App_Start;

    [Authorize]
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            Mapper = new WebAppMapper();
        }

        public WebAppMapper Mapper { get; set; }

        /// <summary>
        /// Overriden version to use Json.Net with Camel case
        /// </summary>
        /// <param name="data"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = behavior
            };
        }

        /// <summary>
        /// Overriden version to use Json.Net with Camel case
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected ActionResult RedirectToDefault()
        {
            return RedirectToAction("Index", "Stats");
        }
    }
}