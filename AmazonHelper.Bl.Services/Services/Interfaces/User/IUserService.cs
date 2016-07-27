using AmazonHelper.Business.Models;

namespace AmazonHelper.Business.Services
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<User> ValidateUser(string userName, string password);
    }
}
