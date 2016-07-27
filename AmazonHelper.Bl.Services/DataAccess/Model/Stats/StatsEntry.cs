using System;

namespace AmazonHelper.DataAccess.Models
{
    public enum StatsEntryResult
    {
        BuyPriceBelongsToCompany = 1,
        BuyPriceNotBelongsToCompany = 2
    }

    public class StatsEntry
    {
        public int StatsEntryId { get; set; }

        public int ProductId { get; set; }

        public int GroupId { get; set; }

        public DateTime DateCreated { get; set; }

        public StatsEntryResult Result { get; set; }

        /* retations */
        public virtual Product Product { get; set; }
    }
}
