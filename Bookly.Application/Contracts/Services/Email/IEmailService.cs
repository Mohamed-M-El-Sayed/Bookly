namespace Bookly.Application.Contracts.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
