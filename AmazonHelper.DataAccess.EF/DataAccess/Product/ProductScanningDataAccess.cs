using AmazonHelper.DataAccess.EF;

namespace AmazonHelper.DataAccess.Models
{
    using System.Threading.Tasks;
    using AmazonHelper.DataAccess.Services;

    public class ProductScanningDataAccess : IProductScanningDataAccess
    {
        private readonly IDbContextFactory _contextFactory;

        public ProductScanningDataAccess(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task SaveProductScanningProcess(ProductScanningProcessDto productScanningProcess)
        {
            throw new System.NotImplementedException();
        }
    }
}
