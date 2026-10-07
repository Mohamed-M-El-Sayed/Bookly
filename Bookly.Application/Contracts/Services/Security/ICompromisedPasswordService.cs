namespace Bookly.Application.Contracts.Services.Security;

public interface ICompromisedPasswordService 
{
    Task<bool> IsCompromisedPasswordAsync(string password, CancellationToken cancellationToken = default);
}
