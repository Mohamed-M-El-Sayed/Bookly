using Bookly.Application.Contracts.Services.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Bookly.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;
        public EmailService(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body , CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.SenderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body =new BodyBuilder { HtmlBody = body }.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password, cancellationToken);
            await smtp.SendAsync(message, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
