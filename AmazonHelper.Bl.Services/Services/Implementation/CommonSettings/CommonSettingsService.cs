namespace AmazonHelper.Business.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Data.Entity;
    using Common;
    using DataAccess;
    using Models;

    public class CommonSettingsService : BaseService, ICommonSettingsService
    {
        private readonly IDbContextFactory _contextFactory;
        public CommonSettingsService(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<CommonSettings> GetCommonSettings(int userId)
        {
            try
            {
                using (var dbContext = _contextFactory.GetAmazonHelperContext())
                {
                    try
                    {
                        var settingsDb = await dbContext.CommonSettings.FirstOrDefaultAsync(s => s.UserId == userId).ConfigureAwait(false);

                        if (settingsDb == null)
                        {
                            throw new DatabaseObjectNotFoundException();
                        }

                        return Mapper.MapCommonSettings(settingsDb);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveCommonSettings(CommonSettings settings)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    var settingsDb = await dbContext.CommonSettings
                        .FirstOrDefaultAsync(s => s.UserId == settings.UserId).ConfigureAwait(false);

                    if (settingsDb == null)
                    {
                        throw new DatabaseObjectNotFoundException();
                    }

                    settingsDb.CompanyName = settings.CompanyName;
                    settingsDb.Email = settings.Email;

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
