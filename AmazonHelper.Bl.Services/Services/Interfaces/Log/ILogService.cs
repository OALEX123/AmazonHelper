using System.Collections.Generic;
using System.Threading.Tasks;
using AmazonHelper.Business.Models;
using AmazonHelper.Common;

namespace AmazonHelper.Business.Services
{
    public interface ILogService
    {
        Task SaveLogAsync(LogEntry logEntry);

        Task<PagedResult<LogEntry>> GetLogsAsync(LogCriteria criteria, PagingParams pagingParams);
    }
}
