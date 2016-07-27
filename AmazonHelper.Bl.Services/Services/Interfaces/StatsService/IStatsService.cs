namespace AmazonHelper.Business.Services
{
    using System.Threading.Tasks;
    using Common;
    using Models;

    public enum StatsCustomAction
    {
        RemoveForLastWeek = 1,
        RemoveAll = 2
    }

    public interface IStatsService
    {
        Task<PagedResult<StatsEntryGrouped>> GetStatsForAllProducts(PagingParams pagingParams, StatsCriteria criteria);
        Task SaveStatEntry(StatsEntry statsEntry);
        Task CustomAction(StatsCustomAction action);
    }
}