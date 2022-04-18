using SendGrid;
using SendGrid.Helpers.Mail;

namespace IdentityDemo_Api.Services
{
    public class MailGridService : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailGridService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration.GetValue<string>("SendGridAPIKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("matevos.grigoryan98@gmail.com", "JWT Auth Demo");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
