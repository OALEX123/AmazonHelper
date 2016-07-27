using System.Collections.Generic;

namespace AmazonHelper.DataAccess.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Asin { get; set; }

        public IEnumerable<ProductScanningProcessDto> ScanningProcesses { get; set; }
    }
}
