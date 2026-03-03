using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using VAlgo.Modules.Identity.Application.Abstractions.Communication;

namespace VAlgo.Modules.Identity.Infrastructure.Communication
{
    public sealed class SmtpEmailSender : IEmailSender
    {
        private readonly EmailOptions _options;

        public SmtpEmailSender(IOptions<EmailOptions> options)
            => _options = options.Value;

        public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_options.FromEmail, _options.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            using var client = new SmtpClient(_options.SmtpHost, _options.SmtpPort)
            {
                Credentials = new NetworkCredential(_options.Username, _options.Password),
                EnableSsl = _options.UseSsl
            };

            await client.SendMailAsync(message);
        }
    }
}