using System;
using AmazonHelper.Infrastructure.Email;

namespace AmazonHelper.Business.Services
{
    using System.Threading.Tasks;

    public class EmailService: IEmailService
    {
        public async Task NotifyBuyPriceNotBelongToCompany(string emailToNotify, string asin, string companyName)
        {
            try
            {
                string subject = $"{asin}";
                string message = $"Asin {asin} buy price does not belong to company {companyName}";
                string emailFrom = System.Configuration.ConfigurationManager.AppSettings["notificationFromEmail"];
                string passwordFrom = System.Configuration.ConfigurationManager.AppSettings["notificationFromPassword"];

                new EmailBuilder()
                    .From(emailFrom)
                    .To(emailToNotify)
                    .Subject(subject)
                    .Body(message)
                    //.UseGmailSmtpServer()
                    .UseSpecificSmtpServer("mail.ethereum.by", 587, true)
                    .Credentials(emailFrom, passwordFrom)
                    .Send();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
