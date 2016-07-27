using System.Collections.Generic;
using System.Linq;
using AmazonHelper.Common;

namespace AmazonHelper.Business.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Data.Entity;
    using DataAccess;
    using Models;
    using DA = DataAccess.Models;

    public class LogService : BaseService, ILogService
    {
        private readonly IDbContextFactory _contextFactory;
        public LogService(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PagedResult<LogEntry>> GetLogsAsync(LogCriteria criteria, PagingParams pagingParams)
        {
            try
            {
                const int maxLogsToTake = 1000;
                using (var dbContext = _contextFactory.GetAmazonHelperContext())
                {
                    var logsToRemove =
                         await dbContext.LogEntries.OrderByDescending(s => s.LogEntryId)
                                .Skip(maxLogsToTake)
                                .ToListAsync()
                                .ConfigureAwait(false);

                    logsToRemove.ForEach(l=> dbContext.LogEntries.Remove(l));

                    await dbContext.SaveChangesAsync();

                    var logEntries = await dbContext.LogEntries.OrderByDescending(s => s.LogEntryId)
                        .Skip((pagingParams.PageNum - 1) * pagingParams.PageSize)
                        .Take(pagingParams.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);

                    var entries = (from logEntry in logEntries
                            select new LogEntry
                            {
                                DateCreated = logEntry.DateCreated,
                                Message = logEntry.Message,
                                LogEntryId = logEntry.LogEntryId
                            }).ToList();

                    int totalCount = await dbContext.LogEntries.CountAsync();
                    if (totalCount > maxLogsToTake)
                    {
                        totalCount = maxLogsToTake;
                    }

                    return new PagedResult<LogEntry>
                    {
                        Data = entries,
                        TotalCount = totalCount
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveLogAsync(LogEntry logEntry)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    dbContext.LogEntries.Add(new DA.LogEntry
                    {
                        DateCreated = DateTime.UtcNow,
                        Message = logEntry.Message
                    });

                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
