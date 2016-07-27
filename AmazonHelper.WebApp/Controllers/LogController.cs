using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AmazonHelper.Business.Services;
using AmazonHelper.Common;

namespace AmazonHelper.WebApp.Controllers
{
    public class LogController : BaseController
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetLogsPaged(LogCriteria criteria, PagingParams pagingParams)
        {
            try
            {
                var logsResult = await _logService.GetLogsAsync(criteria, pagingParams);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        logs = logsResult.Data,
                        logsTotalCount = logsResult.TotalCount
                    }
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
    }
}