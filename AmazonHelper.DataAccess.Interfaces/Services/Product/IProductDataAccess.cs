namespace AmazonHelper.DataAccess.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Models;

    public interface IProductDataAccess
    {
        Task SaveProductAsync(ProductDto product);

        Task RemoveProductAsync(int productId);

        Task<IEnumerable<ProductDto>> GetProductsPagedAsync(ProductCriteriaDto criteria, PagingParams pagingParams);

        Task<int> GetProductsTotalCount();
    }
}
