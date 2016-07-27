namespace AmazonHelper.DataAccess.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface IUserDataAccess
    {
        Task<UserDto> ValidateUser(string userName, string password);
    }
}
