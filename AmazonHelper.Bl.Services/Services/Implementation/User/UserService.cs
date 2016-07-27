namespace AmazonHelper.Business.Services
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Models;

    public class UserService : BaseService, IUserService
    {
        private readonly IDbContextFactory _contextFactory;
        public UserService(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User> ValidateUser(string userName, string password)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                try
                {
                    var userDb = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password).ConfigureAwait(false);

                    if (userDb == null)
                    {
                        return null;
                    }

                    return Mapper.MapUser(userDb);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
