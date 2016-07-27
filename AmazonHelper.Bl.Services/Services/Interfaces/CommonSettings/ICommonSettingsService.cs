namespace AmazonHelper.Business.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface ICommonSettingsService
    {
        Task<CommonSettings> GetCommonSettings(int userId);
        Task SaveCommonSettings(CommonSettings settings);
    }
}
