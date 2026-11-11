using Bookly.Application.Common.Behaviors;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Bookly.Application.DependancyInjection;

public static class DependencyInjection
{
    public static void AddApplicaiton(this IServiceCollection services)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(applicationAssembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(CacheInvalidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(applicationAssembly);
        TypeAdapterConfig.GlobalSettings.Scan(applicationAssembly);
    }
}
