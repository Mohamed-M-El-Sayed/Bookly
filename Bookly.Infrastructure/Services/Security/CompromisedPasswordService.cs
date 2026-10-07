using Bookly.Application.Contracts.Services.Security;
using System.Security.Cryptography;
using System.Text;

namespace Bookly.Infrastructure.Services.Security;

public class CompromisedPasswordService : ICompromisedPasswordService
{
    private readonly HttpClient _httpClient;
    public CompromisedPasswordService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<bool> IsCompromisedPasswordAsync(string password, CancellationToken cancellationToken = default)
    {
        // hash passwrod using SHA-1
        var hash = SHA1.HashData(Encoding.UTF8.GetBytes(password));
        var hashString = Convert.ToHexString(hash);
        // split hash into prefix and suffix
        var prefix = hashString.Substring(0, 5);
        var suffix = hashString.Substring(5);   
        using var response = await _httpClient
            .GetAsync($"range/{prefix}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var lines = responseContent.Split("\n" , StringSplitOptions.RemoveEmptyEntries);
        foreach(var line in lines)
        {
            var parts = line.Split(":", 2);
            if(parts.Length == 2 && parts[0].Equals(suffix, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}   
