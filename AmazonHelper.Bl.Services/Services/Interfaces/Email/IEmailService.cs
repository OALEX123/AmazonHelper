namespace AmazonHelper.Business.Services
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task NotifyBuyPriceNotBelongToCompany(string emailToNotify, string asin, string companyName);
    }
}
