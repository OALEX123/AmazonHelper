namespace AmazonHelper.Business.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Asin { get; set; }

        public int GroupId { get; set; }

        public bool IsActive { get; set; }

        public bool IsNotificationEnabled { get; set; }

        public ProductGroup Group { get; set; }
    }
}
