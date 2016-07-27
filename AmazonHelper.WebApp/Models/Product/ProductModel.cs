using System.ComponentModel.DataAnnotations;

namespace AmazonHelper.WebApp.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        [Required]
        public string Asin { get; set; }

        [Range(minimum: 1, maximum: 3, ErrorMessage = "Group is not recognized")]
        public int GroupId { get; set; }

        public bool IsActive { get; set; }

        public bool IsNotificationEnabled { get; set; }
    }
}