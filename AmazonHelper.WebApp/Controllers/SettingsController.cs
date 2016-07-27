namespace AmazonHelper.WebApp.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Business.Models;
    using Business.Services;

    public class SettingsController : BaseController
    {
        private readonly ICommonSettingsService _commonSettingsService;

        public SettingsController(ICommonSettingsService commonSettingsService)
        {
            _commonSettingsService = commonSettingsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCommonSettings()
        {
            try
            {
                var settings = await _commonSettingsService.GetCommonSettings(1);

                return Json(new
                {
                    success = true,
                    data = settings
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.GetBaseException().Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<JsonResult> SaveCommonSettings(CommonSettings settings)
        {
            try
            {
                await _commonSettingsService.SaveCommonSettings(settings);

                return Json(new
                {
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.GetBaseException().Message
                });
            }
        }
    }
}