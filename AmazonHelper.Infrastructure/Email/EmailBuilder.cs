using System;

namespace AmazonHelper.Infrastructure.Email
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;

    public class EmailBuilder
    {
        private readonly SmtpClient _smtpClient;
        private readonly MailMessage _message;

        public EmailBuilder()
        {
            _smtpClient = new SmtpClient();
            _message = new MailMessage();
        }
        public EmailBuilder To(string email)
        {
            _message.To.Add(email);
            return this;
        }

        public EmailBuilder To(IEnumerable<string> emails)
        {
            emails.ToList().ForEach(e => _message.To.Add(new MailAddress(e)));
            return this;
        }

        public EmailBuilder From(string email)
        {
            _message.From = new MailAddress(email);
            return this;
        }

        public EmailBuilder AttachFiles()
        {
            // attachment logic
            return this;
        }
        public EmailBuilder Credentials(string userName, string password)
        {
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential(userName, password);
            return this;
        }

        public EmailBuilder UseGmailSmtpServer()
        {
            _smtpClient.Host = "smtp.gmail.com";
            _smtpClient.Port = 587;
            _smtpClient.EnableSsl = true;
            return this;
        }

        public EmailBuilder UseSpecificSmtpServer(string smtpHost, int port = 25, bool enableSsl = false)
        {
            _smtpClient.Host = smtpHost;
            _smtpClient.Port = port;
            _smtpClient.EnableSsl = enableSsl;

            return this;
        }

        public EmailBuilder Subject(string subject)
        {
            _message.Subject = subject;
            return this;
        }

        public EmailBuilder Body(string body)
        {
            _message.Body = body;
            return this;
        }

        public void Send()
        {
            try
            {
                _smtpClient.Send(_message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
