namespace AmazonHelper.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Business.Models;
    using Business.Services;
    using Common;
    using Models;

    public class StatsController : BaseController
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetStatsPaged(PagingParams pagingParams, StatsCriteria criteria)
        {
            try
            {
                //var products = await _productService.GetProductsPagedAsync(new ProductCriteria(), new PagingParams { PageNum = 1, PageSize = 10000 });
                //var groups = await _productService.GetProductGroups();
                var statsEntriesGrouped = await _statsService.GetStatsForAllProducts(pagingParams, criteria);

                var statsEntriesGroupedByProduct = from s in statsEntriesGrouped.Data
                                                   group s by s.ProductId into g
                                                   select new { ProductId = g.Key, Groups = g.ToList() };

                var statsModels = new List<StatsModel>();

                foreach (var entry in statsEntriesGroupedByProduct)
                {
                    var statsModel = new StatsModel
                    {
                        ProductId = entry.ProductId
                    };

                    var groups = new List<StatsEntryGroupedByGroupModel>();
                    foreach (var entryGrouped in entry.Groups)
                    {
                        statsModel.Asin = entryGrouped.Asin;

                        var statsByDayModels = new List<StatsByDayModel>
                        {
                            new StatsByDayModel
                            {
                                Date = entryGrouped.Monday.DateCreated,
                                DayName = "Monday",
                                TotalSuccessed = entryGrouped.Monday.SearchesSuccessed,
                                TotalFailed = entryGrouped.Monday.SearchesFailed
                            },
                            new StatsByDayModel
                            {
                                Date = entryGrouped.Tuesday.DateCreated,
                                DayName = "Tuesday",
                                TotalSuccessed = entryGrouped.Tuesday.SearchesSuccessed,
                                TotalFailed = entryGrouped.Tuesday.SearchesFailed
                            },
                            new StatsByDayModel
                            {
                                Date = entryGrouped.Wednesday.DateCreated,
                                DayName = "Wednesday",
                                TotalSuccessed = entryGrouped.Wednesday.SearchesSuccessed,
                                TotalFailed = entryGrouped.Wednesday.SearchesFailed
                            },
                            new StatsByDayModel
                            {
                                Date = entryGrouped.Thursday.DateCreated,
                                DayName = "Thursday",
                                TotalSuccessed = entryGrouped.Thursday.SearchesSuccessed,
                                TotalFailed = entryGrouped.Thursday.SearchesFailed
                            },
                            new StatsByDayModel
                            {
                                Date = entryGrouped.Friday.DateCreated,
                                DayName = "Friday",
                                TotalSuccessed = entryGrouped.Friday.SearchesSuccessed,
                                TotalFailed = entryGrouped.Friday.SearchesFailed
                            },
                            new StatsByDayModel
                            {
                                Date = entryGrouped.Saturday.DateCreated,
                                DayName = "Saturday",
                                TotalSuccessed = entryGrouped.Saturday.SearchesSuccessed,
                                TotalFailed = entryGrouped.Saturday.SearchesFailed
                            },
                            new StatsByDayModel
                            {
                                Date = entryGrouped.Sunday.DateCreated,
                                DayName = "Sunday",
                                TotalSuccessed = entryGrouped.Sunday.SearchesSuccessed,
                                TotalFailed = entryGrouped.Sunday.SearchesFailed
                            }
                        };

                        groups.Add(new StatsEntryGroupedByGroupModel
                        {
                            GroupName = entryGrouped.GroupName,
                            StartDate = entryGrouped.FirstEntryDate.ToString(),
                            EndDate = entryGrouped.LastEntryDate?.ToString() ?? string.Empty,
                            TotalFailed = entryGrouped.TotalFailed,
                            TotalSuccessed = entryGrouped.TotalSuccessed,
                            StatsByDay = statsByDayModels.OrderByDescending(sbd => sbd.Date)
                        });

                    }

                    statsModel.Groups = groups;
                    statsModels.Add(statsModel);
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        stats = statsModels,
                        statsTotalCount = statsEntriesGrouped.TotalCount
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

        [HttpPost]
        public async Task<JsonResult> CustomAction(StatsCustomAction action)
        {
            try
            {
                await _statsService.CustomAction(action);

                return Json(new
                {
                    success = true
                });
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