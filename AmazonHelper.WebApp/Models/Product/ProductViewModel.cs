namespace AmazonHelper.WebApp.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Asin { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public bool IsActive { get; set; }

        public bool IsNotificationEnabled { get; set; }

        public string IsActiveState { get; set; }

        public string IsNotificationEnabledState { get; set; }
    }
}