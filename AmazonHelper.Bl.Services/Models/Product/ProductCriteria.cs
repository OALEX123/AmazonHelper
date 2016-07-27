namespace AmazonHelper.Business.Models
{
    public enum ProductCustomAction
    {
        Activate = 1,
        Deactivate = 2
    }

    public class ProductCriteria
    {
        public string Asin { get; set; }
        public int GroupId { get; set; }
        public bool? IsActive { get; set; }
    }
}
