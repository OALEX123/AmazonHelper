using System;

namespace AmazonHelper.Business.Models
{
    public class GetProductStatsResult
    {
        public int ProductId { get; set; }

        public int GroudId { get; set; }

        public string Asin { get; set; }

        public string GroupName { get; set; }

        public DateTime MondayDate { get; set; }

        public DateTime TuesdayDate { get; set; }

        public DateTime WednesdayDate { get; set; }

        public DateTime ThirsdayDate { get; set; }

        public DateTime FridayDate { get; set; }

        public DateTime SaturdayDate { get; set; }

        public DateTime SundayDate { get; set; }

        public DateTime FirstEntryDate { get; set; }

        public DateTime LastEntryDate { get; set; }

        public int MondaySuccessed { get; set; }

        public int MondayFailed { get; set; }

        public int TuesdaySuccessed { get; set; }

        public int TuesdayFailed { get; set; }

        public int WednesdaySuccessed { get; set; }

        public int WednesdayFailed { get; set; }

        public int ThursdaySuccessed { get; set; }

        public int ThursdayFailed { get; set; }

        public int FridaySuccessed { get; set; }

        public int FridayFailed { get; set; }

        public int SaturdaySuccessed { get; set; }

        public int SaturdayFailed { get; set; }

        public int SundaySuccessed { get; set; }

        public int SundayFailed { get; set; }

        public int TotalSuccessed { get; set; }

        public int TotalFailed { get; set; }
    }
}
