using System.Collections.Generic;

namespace AmazonHelper.DataAccess.Models
{
    public class ProductGroup
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int Interval { get; set; }

        public ICollection<Product> Products { get; set; }

        //public ICollection<StatsEntry> StatsEntries { get; set; }
    }
}
