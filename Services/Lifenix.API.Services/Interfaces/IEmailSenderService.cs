namespace Lifenix.API.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IEmailSenderService
    {
        Task SendEmail(string subject, string toEmail, string username, string message);
    }
}
