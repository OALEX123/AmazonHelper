namespace AmazonHelper.Business.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Models;

    public interface IProductService
    {
        Task SaveProductAsync(Product product);

        Task RemoveProductAsync(int productId);

        Task<PagedResult<Product>> GetProductsPagedAsync(ProductCriteria criteria, PagingParams pagingParams);

        Task<Product> GetProductAsync(int productId);

        Task<int> GetProductsTotalCount();

        Task<IEnumerable<ProductGroup>> GetProductGroups();

        Task CustomAction(int[] productIds, ProductCustomAction action);
    }
}
