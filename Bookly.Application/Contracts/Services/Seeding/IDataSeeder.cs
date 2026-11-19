namespace Bookly.Application.Contracts.Services.Seeding;

public interface IDataSeeder
{
    public Task SeedAsync(CancellationToken cancellationToken = default);
}
