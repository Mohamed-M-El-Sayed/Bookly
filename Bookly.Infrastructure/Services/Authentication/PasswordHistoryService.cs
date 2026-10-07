using Bookly.Domain.Entities.Identity;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Authentication;

public class PasswordHistoryService
{
    private readonly AppDbContext _appDbContext;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public PasswordHistoryService(AppDbContext appDbContext , IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _appDbContext = appDbContext;
        _passwordHasher = passwordHasher;
    }
    private const int MaxPasswordHistoryCount = 4;
    public async Task<bool> IsPasswordReusedAsync(
    ApplicationUser user,
    string newPassword,
    CancellationToken cancellationToken = default)
    {
        var currentPasswordResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash!,
            newPassword);

        if (currentPasswordResult == PasswordVerificationResult.Success)
            return true;

        var passwordHistories = await _appDbContext.PasswordHistories
            .Where(x => x.UserId == user.Id)
            .OrderByDescending(x => x.CreatedAt)
            .Take(MaxPasswordHistoryCount)
            .ToListAsync(cancellationToken);

        foreach (var passwordHistory in passwordHistories)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                passwordHistory.PasswordHash,
                newPassword);

            if (result == PasswordVerificationResult.Success)
                return true;
        }

        return false;
    }
    public async Task AddPasswordToHistoryAsync(
      ApplicationUser user,
      string passwordHash,
      CancellationToken cancellationToken = default)
    {
        var history = await _appDbContext.PasswordHistories
            .Where(x => x.UserId == user.Id)
            .OrderByDescending(x => x.CreatedAt)
            .Take(MaxPasswordHistoryCount)
            .ToListAsync(cancellationToken);

        if (history.Count >= MaxPasswordHistoryCount)
        {
            var oldestHistory = history.Last();

            _appDbContext.PasswordHistories.Remove(oldestHistory);
        }

        user.AddPasswordHistory(passwordHash);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }



}