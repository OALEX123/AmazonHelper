namespace AmazonHelper.Business.Models
{
    using System;

    public enum StatsEntryResult
    {
        BuyPriceBelongsToCompany = 1,
        BuyPriceNotBelongsToCompany = 2
    }

    public class StatsCriteria
    {
        public string Asin { get; set; }
    }

    public class StatsEntry
    {
        public long StatsEntryId { get; set; }

        public int ProductId { get; set; }

        public int GroupId { get; set; }

        public DateTime DateCreated { get; set; }

        public StatsEntryResult Result { get; set; }
    }

    public class StatsEntryGrouped
    {
        public int ProductId { get; set; }

        public int GroupId { get; set; }

        public string Asin { get; set; }

        public string GroupName { get; set; }

        public DateTime FirstEntryDate { get; set; }

        public DateTime? LastEntryDate { get; set; }

        public StatEntryGropedByDay Monday { get; set; }

        public StatEntryGropedByDay Tuesday { get; set; }

        public StatEntryGropedByDay Wednesday { get; set; }

        public StatEntryGropedByDay Thursday { get; set; }

        public StatEntryGropedByDay Friday { get; set; }

        public StatEntryGropedByDay Saturday { get; set; }

        public StatEntryGropedByDay Sunday { get; set; }

        public int TotalSuccessed { get; set; }

        public int TotalFailed { get; set; }
    }

    public class StatEntryGropedByDay
    {
        public DateTime DateCreated { get; set; }

        public int SearchesFailed { get; set; }

        public int SearchesSuccessed { get; set; }

        public int SearchesTotal { get; set; }
    }
}
