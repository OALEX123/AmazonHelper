using System.Data;
using System.Data.SqlClient;
using AmazonHelper.Common;

namespace AmazonHelper.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DA = DataAccess.Models;
    using DataAccess;
    using Models;

    public class StatsService : BaseService, IStatsService
    {
        private readonly IDbContextFactory _contextFactory;

        #region query
        private readonly string _getStatsQuery = @"with d as(select n
    from(values(1),(2),(3),(4),(5),(6),(7)) as t(n)
	),

	 dates as 
	(
	select
    cast(dateadd(day,-n, dateadd(day,1,GETUTCDATE())) as date) dt, 
	  case datepart(weekday, dateadd(day,-n, dateadd(day,1,GETUTCDATE()))) 
		when 1 then 'S'
		when 2 then 'M'
		when 3 then 'T'
		when 4 then 'W'
		when 5 then 'Th'
		when 6 then 'F'
		when 7 then 'Sa'
	  end wd

        from d
	),
     ProductsFiltered as(

        select
        p.ProductId,
        row_number() over(order by p.ProductId desc) as RowNumber
        from Products p
        where (p.Asin like '%' + @asin + '%' or @asin is null) 
	)


    select
    s.ProductId,
    s.GroupId,
    p.[Asin],
    g.[GroupName],
    min(s.DateCreated) as 'FirstEntryDate',
    max(s.DateCreated) as 'LastEntryDate',
    sum(case when s.Result = 1 then 1 else 0 end) as 'TotalSuccessed',
    sum(case when s.Result = 2 then 1 else 0 end) as 'TotalFailed',
	-- MONDAY --
	(select cast(dates.dt as datetime) from dates where dates.wd = 'M') as 'MondayDate',
    sum(case when (s.Result = 1 and DATEPART(weekday, s.DateCreated) = 2) then 1 else 0 end) as 'MondaySuccessed',

    sum(case when (s.Result = 2 and DATEPART(weekday, s.DateCreated) = 2) then 1 else 0 end) as 'MondayFailed',
	-- TUESDAY --
	(select cast(dates.dt as datetime)  from dates where dates.wd = 'T') as 'TuesdayDate',
    sum(case when (s.Result = 1 and DATEPART(weekday, s.DateCreated) = 3) then 1 else 0 end) as 'TuesdaySuccessed',

    sum(case when (s.Result = 2 and DATEPART(weekday, s.DateCreated) = 3) then 1 else 0 end) as 'TuesdayFailed',
	-- WEDNESDAY --
	(select cast(dates.dt as datetime) from dates where dates.wd = 'W') as 'WednesdayDate',
    sum(case when (s.Result = 1 and DATEPART(weekday, s.DateCreated) = 4) then 1 else 0 end) as 'WednesdaySuccessed',

    sum(case when (s.Result = 2 and DATEPART(weekday, s.DateCreated) = 4) then 1 else 0 end) as 'WednesdayFailed',
	-- THURSDAY --
	(select cast(dates.dt as datetime) from dates where dates.wd = 'Th') as 'ThirsdayDate',
    sum(case when (s.Result = 1 and DATEPART(weekday, s.DateCreated) = 5) then 1 else 0 end) as 'ThursdaySuccessed',

    sum(case when (s.Result = 2 and DATEPART(weekday, s.DateCreated) = 5) then 1 else 0 end) as 'ThursdayFailed',
	-- FRIDAY --
	(select cast(dates.dt as datetime) from dates where dates.wd = 'F') as 'FridayDate',
    sum(case when (s.Result = 1 and DATEPART(weekday, s.DateCreated) = 6) then 1 else 0 end) as 'FridaySuccessed',

    sum(case when (s.Result = 2 and DATEPART(weekday, s.DateCreated) = 6) then 1 else 0 end) as 'FridayFailed',
	-- SATURDAY --
	(select cast(dates.dt as datetime) from dates where dates.wd = 'Sa') as 'SaturdayDate',
    sum(case when (s.Result = 1 and DATEPART(weekday, s.DateCreated) = 7) then 1 else 0 end) as 'SaturdaySuccessed',

    sum(case when (s.Result = 2 and DATEPART(weekday, s.DateCreated) = 7) then 1 else 0 end) as 'SaturdayFailed',
	-- SUNDAY --
	(select cast(dates.dt as datetime) from dates where dates.wd = 'S') as 'SundayDate',
    sum(case when (s.Result = 1 and DATEPART(weekday, s.DateCreated) = 1) then 1 else 0 end) as 'SundaySuccessed',

    sum(case when (s.Result = 2 and DATEPART(weekday, s.DateCreated) = 1) then 1 else 0 end) as 'SundayFailed',
	-- Total Records --

    count(1) as 'Total'
	from StatsEntries s
    inner join Products p on p.ProductId = s.ProductId
    inner join ProductsFiltered pf on p.ProductId = pf.ProductId
    inner join ProductGroups g on g.GroupId = s.GroupId
    where DATEDIFF(DAY, s.DateCreated, GETUTCDATE()) <= 7 and pf.RowNumber between @startIndex and @endIndex
 
     group by s.GroupId, s.ProductId, p.[Asin], g.GroupName";
        #endregion

        public StatsService(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PagedResult<StatsEntryGrouped>> GetStatsForAllProducts(PagingParams pagingParams, StatsCriteria criteria)
        {
            try
            {
                using (var dbContext = new AmazonHelperDb())
                {
                    int startIndex = (pagingParams.PageNum - 1) * pagingParams.PageSize + 1;
                    int endIndex = startIndex + pagingParams.PageSize - 1;

                    var asinParameter = new SqlParameter("@asin", SqlDbType.NVarChar);
                    if (!string.IsNullOrEmpty(criteria.Asin))
                    {
                        asinParameter.Value = criteria.Asin;
                    }
                    else
                    {
                        asinParameter.Value = DBNull.Value;
                    }

                    var queryResult = await dbContext.Database.SqlQuery<GetProductStatsResult>(_getStatsQuery, 
                        asinParameter,
                        new SqlParameter("@startIndex", startIndex),
                        new SqlParameter("@endIndex", endIndex))
                        .ToListAsync();

                    var grouped = queryResult.Select(r => new StatsEntryGrouped
                    {
                        ProductId = r.ProductId,
                        GroupId = r.GroudId,
                        Asin = r.Asin,
                        GroupName = r.GroupName,
                        FirstEntryDate = r.FirstEntryDate,
                        LastEntryDate = r.LastEntryDate,
                        TotalFailed = r.TotalFailed,
                        TotalSuccessed = r.TotalSuccessed,
                        Monday = new StatEntryGropedByDay
                        {
                            SearchesFailed = r.MondayFailed,
                            SearchesSuccessed = r.MondaySuccessed,
                            DateCreated = r.MondayDate
                        },
                        Tuesday = new StatEntryGropedByDay
                        {
                            SearchesFailed = r.TuesdayFailed,
                            SearchesSuccessed = r.TuesdaySuccessed,
                            DateCreated = r.TuesdayDate
                        },
                        Wednesday = new StatEntryGropedByDay
                        {
                            SearchesFailed = r.WednesdayFailed,
                            SearchesSuccessed = r.WednesdaySuccessed,
                            DateCreated = r.WednesdayDate
                        },
                        Thursday = new StatEntryGropedByDay
                        {
                            SearchesFailed = r.ThursdayFailed,
                            SearchesSuccessed = r.ThursdaySuccessed,
                            DateCreated = r.ThirsdayDate
                        },
                        Friday = new StatEntryGropedByDay
                        {
                            SearchesFailed = r.FridayFailed,
                            SearchesSuccessed = r.FridaySuccessed,
                            DateCreated = r.FridayDate
                        },
                        Saturday = new StatEntryGropedByDay
                        {
                            SearchesFailed = r.SaturdayFailed,
                            SearchesSuccessed = r.SaturdaySuccessed,
                            DateCreated = r.SaturdayDate
                        },
                        Sunday = new StatEntryGropedByDay
                        {
                            SearchesFailed = r.SundayFailed,
                            SearchesSuccessed = r.SundaySuccessed,
                            DateCreated = r.SundayDate
                        }
                    });

                    string totalCountQuery = @"select count(distinct s.ProductId) from[dbo].[StatsEntries] s
                                               inner join[dbo].[Products] p on p.ProductId = s.ProductId
                                               where (p.Asin like '%' + @asin + '%' or @asin is null)";

                    var asinParameter2 = new SqlParameter("@asin", SqlDbType.NVarChar);
                    if (!string.IsNullOrEmpty(criteria.Asin))
                    {
                        asinParameter2.Value = criteria.Asin;
                    }
                    else
                    {
                        asinParameter2.Value = DBNull.Value;
                    }

                    var totalCount = await dbContext.Database
                        .SqlQuery<int>(totalCountQuery, asinParameter2)
                        .FirstOrDefaultAsync().ConfigureAwait(false);
                    
                    var result = new PagedResult<StatsEntryGrouped>
                    {
                        Data = grouped.ToList(),
                        TotalCount = totalCount //await dbContext.Products.CountAsync().ConfigureAwait(false)
                    };

                    return result;

                    //var stats = await dbContext.StatsEntries.OrderBy(s => s.StatsEntryId)
                    //    .Skip((pagingParams.PageNum - 1) * pagingParams.PageSize)
                    //    .Take(pagingParams.PageSize)
                    //    .ToListAsync()
                    //    .ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveStatEntry(StatsEntry statsEntry)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    //var statsEntryDb = await dbContext.StatsEntries
                    //    .FirstOrDefaultAsync(s => s.ProductId == productId && s.GroupId == groupId)
                    //    .ConfigureAwait(false);

                    dbContext.StatsEntries.Add(new DA.StatsEntry
                    {
                        ProductId = statsEntry.ProductId,
                        GroupId = statsEntry.GroupId,
                        DateCreated = DateTime.UtcNow,
                        Result = (DA.StatsEntryResult)(int)statsEntry.Result
                    });
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task CustomAction(StatsCustomAction action)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    switch (action)
                    {
                        case StatsCustomAction.RemoveForLastWeek:
                            var timeSpan = DateTime.Now.AddDays(-7);
                            var statsForLastWeek = await dbContext.StatsEntries
                                .Where(s => s.DateCreated > timeSpan).ToListAsync().ConfigureAwait(false);
                            statsForLastWeek.ForEach(s => dbContext.StatsEntries.Remove(s));
                            break;
                        case StatsCustomAction.RemoveAll:
                            var stats = await dbContext.StatsEntries.ToListAsync().ConfigureAwait(false);
                            stats.ForEach(s => dbContext.StatsEntries.Remove(s));
                            break;
                    }

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
