namespace AmazonHelper.DataAccess.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Common;
    using EF.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using EF;

    public class ProductDataAccess : BaseDataAccess, IProductDataAccess
    {
        private readonly IDbContextFactory _contextFactory;

        public ProductDataAccess(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task SaveProductAsync(ProductDto product)
        {
            var productToSave = Mapper.MapProduct(product);

            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    if (productToSave.ProductId > 0)
                    {
                        var productDb = await dbContext.Products.Include(p => p.ScanningProcesses)
                            .FirstOrDefaultAsync(p => p.ProductId == productToSave.ProductId).ConfigureAwait(false);

                        productDb.Asin = productToSave.Asin;
                        productDb.ProductName = productToSave.ProductName;
                        productDb.ScanningProcesses.FirstOrDefault().ScanningGroup =
                            productToSave.ScanningProcesses.FirstOrDefault().ScanningGroup;
                    }
                    else
                    {
                        dbContext.Products.Add(productToSave);
                        dbContext.ProductScanningProcesses.Add(new ProductScanningProcess
                        {
                            Product = productToSave,
                            ScanningEvent = EF.Models.ProductScanningEvent.PriceMismatch,
                            ScanningGroup = productToSave.ScanningProcesses.FirstOrDefault().ScanningGroup,
                            IsActive = true,
                        });
                    }

                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task RemoveProductAsync(int productId)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    var product = await dbContext.Products
                    .FirstOrDefaultAsync(p => p.ProductId == productId).ConfigureAwait(false);

                    if (product == null)
                    {
                        throw new DatabaseObjectNotFoundException();
                    }

                    dbContext.Products.Remove(product);

                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsPagedAsync(string asin, PagingParams pagingParams = null)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    var query = dbContext.Products;
                    if (!string.IsNullOrEmpty(criteria.Asin))
                    {
                        query = (IDbSet<Product>)query.Where(p => p.Asin.Contains(criteria.Asin));
                    }

                    var products = await query.OrderBy(s => s.ProductId)
                    .Skip((pagingParams.PageNum - 1) * pagingParams.PageSize)
                    .Take(pagingParams.PageSize).Include(p => p.ScanningProcesses).ToListAsync().ConfigureAwait(false);

                    return products.Select(p => Mapper.MapProduct(p)).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task<int> GetProductsTotalCount()
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    return await dbContext.Products.CountAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }
}
