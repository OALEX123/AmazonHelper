namespace AmazonHelper.DataAccess.Models
{
    using Common;

    public enum ProductScanningGroup
    {
        Every10Minutes = 1,
        EveryHour = 2,
        EveryDay = 3
    }

    public enum ProductScanningEvent
    {
        PriceMismatch = 1
    }

    public class ProductScanningProcessDto
    {
        public int ProductScanningProcessId { get; set; }

        public int ProductId { get; set; }

        public ProductScanningGroup ScanningGroup { get; set; }

        public ProductScanningEvent ScanningEvent { get; set; }

        public bool IsActive { get; set; }
    }
}
