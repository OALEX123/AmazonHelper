using System;
using System.Linq;

namespace AmazonHelper.WebApp.Models
{
    using System.Collections.Generic;

    public class StatsModel
    {
        public int ProductId { get; set; }

        public string Asin { get; set; }

        public int TotalSuccessed
        {
            get
            {
                return Groups.Sum(@group => @group.TotalSuccessed);
            }
        }

        public int Total
        {
            get
            {
                return Groups.Sum(@group => @group.Total);
            }
        }
        public IEnumerable<StatsEntryGroupedByGroupModel> Groups { get; set; }
    }

    public class StatsEntryGroupedByGroupModel
    {
        public string GroupName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int TotalSuccessed { get; set; }

        public int TotalFailed { get; set; }

        public int Total => TotalFailed + TotalSuccessed;

        public IEnumerable<StatsByDayModel> StatsByDay { get; set; }
    }

    public class StatsByDayModel
    {
        public string DayName { get; set; }

        public DateTime Date { get; set; }

        public string FullName => $"{DayName} {Date.ToString("dd-MM-yyyy")}";

        public int TotalSuccessed { get; set; }

        public int TotalFailed { get; set; }

        public int Total => TotalFailed + TotalSuccessed;
    }
}