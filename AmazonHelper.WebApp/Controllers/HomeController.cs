using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AmazonHelper.Engine.Parser;

namespace AmazonHelper.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RunLongQuery()
        {
            try
            {
                Thread.Sleep(10000);

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false
                });
            }
        }

        //[HttpPost]
        //public async Task<JsonResult> CheckAmazon(string asin, string companyName)
        //{
        //    try
        //    {
        //        var result = await AmazonParser.ParseBuyPriceAsync(asin, companyName);
        //        string message = result. ? "Company is on main page" : "Company is NOT on main page";

        //        return Json(new
        //        {
        //            success = true,
        //            message
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new
        //        {
        //            success = false
        //        });
        //    }

        //}

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}