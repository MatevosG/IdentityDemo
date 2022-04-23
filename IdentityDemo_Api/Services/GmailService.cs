using System.Net;
using System.Net.Mail;

namespace IdentityDemo_Api.Services
{
    public class GmailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public GmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //TODO: calls   1 register 2 confirm 3 login 4 forgote 5 reset
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var from = _configuration.GetValue<string>("From");
            var fromAddress = _configuration.GetValue<string>("FromAddress");
            var fromAddressMail = new MailAddress(fromAddress, from);
            var toAddress = new MailAddress(toEmail, "To client");
            var fromPassword = _configuration.GetValue<string>("FromMailPassword");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddressMail.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddressMail, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = content
            })
            {
                smtp.Send(message);
            }
        }
    }
}
