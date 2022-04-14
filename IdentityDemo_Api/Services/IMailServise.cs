namespace IdentityDemo_Api.Services
{
    public interface IMailServise
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
}
