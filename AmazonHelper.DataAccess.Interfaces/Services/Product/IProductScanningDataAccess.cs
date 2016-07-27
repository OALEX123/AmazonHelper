namespace AmazonHelper.DataAccess.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Models;

    public interface IProductScanningDataAccess
    {
        Task SaveProductScanningProcess(ProductScanningProcessDto productScanningProcess);
    }
}
