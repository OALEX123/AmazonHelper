namespace AmazonHelper.Business.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using Common;
    using DataAccess;
    using Models;
    using DA = DataAccess.Models;

    public class ProductService : BaseService, IProductService
    {
        private readonly IDbContextFactory _contextFactory;

        public ProductService(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task SaveProductAsync(Product product)
        {
            var productToSave = Mapper.MapProduct(product);

            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    dbContext.Products.AddOrUpdate(productToSave);
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
            try
            {
                using (var dbContext = _contextFactory.GetAmazonHelperContext())
                {
                    try
                    {
                        var productDb = await dbContext.Products
                            .FirstOrDefaultAsync(p => p.ProductId == productId).ConfigureAwait(false);

                        if (productDb == null)
                        {
                            throw new DatabaseObjectNotFoundException();
                        }

                        var statsDb = await dbContext.StatsEntries
                            .Where(se => se.ProductId == productId).ToListAsync().ConfigureAwait(false);

                        statsDb.ForEach(s => dbContext.StatsEntries.Remove(s));

                        dbContext.Products.Remove(productDb);

                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            try
            {
                using (var dbContext = _contextFactory.GetAmazonHelperContext())
                {
                    try
                    {
                        var productDb = await dbContext.Products
                            .FirstOrDefaultAsync(p => p.ProductId == productId).ConfigureAwait(false);

                        if (productDb == null)
                        {
                            throw new DatabaseObjectNotFoundException();
                        }

                        return Mapper.MapProduct(productDb);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetProductsTotalCount()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedResult<Product>> GetProductsPagedAsync(ProductCriteria criteria, PagingParams pagingParams)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    var query = string.IsNullOrEmpty(criteria.Asin) ? dbContext.Products.OrderByDescending(s => s.ProductId)
                        : dbContext.Products.Where(p => p.Asin.Contains(criteria.Asin));

                    if (criteria.GroupId > 0)
                    {
                        query = query.Where(p => p.GroupId == criteria.GroupId);
                    }

                    if (criteria.IsActive.HasValue)
                    {
                        query = query.Where(p => p.IsActive == criteria.IsActive);
                    }

                    var products = await query.OrderByDescending(s => s.ProductId)
                        .Skip((pagingParams.PageNum - 1) * pagingParams.PageSize)
                        .Take(pagingParams.PageSize)
                        .Include(p => p.Group)
                        .ToListAsync()
                        .ConfigureAwait(false);

                    var result = new PagedResult<Product>
                    {
                        Data = products.Select(p => Mapper.MapProduct(p)).ToList(),
                        TotalCount = await query.CountAsync().ConfigureAwait(false)
                    };

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<ProductGroup>> GetProductGroups()
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    var groups = await dbContext.ProductGroups.ToListAsync().ConfigureAwait(false);
                    return groups.Select(g => Mapper.MapProductGroup(g)).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task CustomAction(int[] productIds, ProductCustomAction action)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    var productsDb = await dbContext.Products
                        .Where(p => productIds.Contains(p.ProductId)).ToListAsync().ConfigureAwait(false);

                    switch (action)
                    {
                        case ProductCustomAction.Activate:
                            productsDb.ForEach(p => p.IsActive = true);
                            break;
                        case ProductCustomAction.Deactivate:
                            productsDb.ForEach(p => p.IsActive = false);
                            break;
                    }

                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
