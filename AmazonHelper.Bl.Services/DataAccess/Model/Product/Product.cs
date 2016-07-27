using System.Collections.Generic;

namespace AmazonHelper.DataAccess.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int GroupId { get; set; }

        public string Asin { get; set; }

        public bool IsActive { get; set; }

        public bool IsNotificationEnabled { get; set; }

        /* relations */
        public virtual ProductGroup Group { get; set; }
        public ICollection<StatsEntry> StatsEntries { get; set; }
    }
}
