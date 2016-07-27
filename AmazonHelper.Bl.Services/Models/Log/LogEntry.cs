using System;

public class LogCriteria
{
    public string Message { get; set; }
}

namespace AmazonHelper.Business.Models
{
    public class LogEntry
    {
        public long LogEntryId { get; set; }

        public string Message { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
