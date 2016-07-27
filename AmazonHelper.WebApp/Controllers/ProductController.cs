using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AmazonHelper.Business.Models;
using AmazonHelper.Business.Services;
using AmazonHelper.Common;
using AmazonHelper.WebApp.Models;
using AmazonHelper.WebApp.Infrastructure;

namespace AmazonHelper.WebApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetProductsPaged(ProductCriteria criteria, PagingParams pagingParams)
        {
            try
            {
                var productResult = await _productService.GetProductsPagedAsync(criteria, pagingParams);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        products = productResult.Data.Select(p => new ProductViewModel
                        {
                            Asin = p.Asin,
                            GroupId = p.GroupId,
                            GroupName = p.Group.GroupName,
                            ProductId = p.ProductId,
                            ProductName = new string(p.ProductName.Take(50).ToArray()) + "...",
                            IsActive = p.IsActive,
                            IsNotificationEnabled = p.IsNotificationEnabled,
                            IsActiveState = p.IsActive ? "Active" : "Deactivated",
                            IsNotificationEnabledState = p.IsNotificationEnabled ? "Yes" : "No"
                        }),
                        productsTotalCount = productResult.TotalCount
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

        [HttpGet]
        public async Task<JsonResult> GetProduct(int productId)
        {
            try
            {
                var productBL = await _productService.GetProductAsync(productId);

                var product = Mapper.MapProduct(productBL);

                return Json(new
                {
                    success = true,
                    data = product
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
        public async Task<JsonResult> SaveProduct(ProductModel product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.ToJsonErrors()
                    });
                }

                var productBL = Mapper.MapProduct(product);

                await _productService.SaveProductAsync(productBL);

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

        [HttpPost]
        public async Task<JsonResult> RemoveProduct(int productId)
        {
            try
            {
                await _productService.RemoveProductAsync(productId);

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

        [HttpPost]
        public async Task<JsonResult> CustomAction(int[] productIds, ProductCustomAction action)
        {
            try
            {
                await _productService.CustomAction(productIds, action);

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

        [HttpGet]
        public async Task<JsonResult> GetProductGroupes()
        {
            try
            {
                var groups = await _productService.GetProductGroups();

                return Json(new
                {
                    data = groups,
                    success = true
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