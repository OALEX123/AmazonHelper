using System;

namespace AmazonHelper.DataAccess.Models
{
    public class LogEntry
    {
        public long LogEntryId { get; set; }

        public string Message { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
