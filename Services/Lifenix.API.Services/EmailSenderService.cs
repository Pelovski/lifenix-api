namespace Lifenix.API.Services
{
    using System;
    using System.Threading.Tasks;
    using Lifenix.API.Services.Interfaces;
    using Microsoft.Extensions.Configuration;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSenderService : IEmailSenderService
    {
        private readonly string apiKey;

        public EmailSenderService(IConfiguration config)
        {
            this.apiKey = config["SendGrid:ApiKey"];
        }

        public async Task SendEmail(string subject, string toEmail, string username, string message)
        {
            var client = new SendGridClient(this.apiKey);
            var from = new EmailAddress("git.pelovski@gmail.com", "LifeNix");
            var to = new EmailAddress(toEmail, username);
            var plainTextContent = message;
            var htmlContent = string.Empty;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}