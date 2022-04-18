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

        //TODO: Move hard coded values to appsettings.json
        //TODO: calls   1 register 2 confirm 3 login 4 forgote 5 reset
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration.GetValue<string>("SendGridAPIKey");

            var fromAddress = new MailAddress("ctestmatos@gmail.com", "From mail service");
            var toAddress = new MailAddress(toEmail, "To client");
            const string fromPassword = "Ctestmatos12!";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
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
