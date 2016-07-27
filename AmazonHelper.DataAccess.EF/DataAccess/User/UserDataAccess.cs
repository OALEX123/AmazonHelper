namespace AmazonHelper.DataAccess.Services
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Models;
    using EF;

    public class UserDataAccess : BaseDataAccess, IUserDataAccess
    {
        private readonly IDbContextFactory _contextFactory;

        public UserDataAccess(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<UserDto> ValidateUser(string userName, string password)
        {
            using (var dbContext = _contextFactory.GetAmazonHelperContext())
            {
                var userDb = await dbContext.Users
                    .FirstOrDefaultAsync(u=>u.UserName == userName && u.Password == password).ConfigureAwait(false);

                if (userDb == null)
                {
                    return null;
                }

                return Mapper.MapUser(userDb);
            }
        }
    }
}
