namespace AmazonHelper.DataAccess.Services
{
    using System.Threading.Tasks;
    using Models;
    using System;
    using System.Data.Entity;
    using Common;
    using EF;

    public class CommonSettingsDataAccess : BaseDataAccess, ICommonSettingsDataAccess
    {
        private readonly IDbContextFactory _contextFactory;

        public CommonSettingsDataAccess(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<CommonSettingsDto> GetCommonSettings(int userId)
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

        public async Task SaveCommonSettings(CommonSettingsDto settings)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    var settingsDb = await dbContext.CommonSettings
                        .FirstOrDefaultAsync(s => s.CommonSettingsId == settings.CommonSettingsId).ConfigureAwait(false);

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
