namespace AmazonHelper.DataAccess.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface ICommonSettingsDataAccess
    {
        Task<CommonSettingsDto> GetCommonSettings(int userId);
        Task SaveCommonSettings(CommonSettingsDto settings);
    }
}
