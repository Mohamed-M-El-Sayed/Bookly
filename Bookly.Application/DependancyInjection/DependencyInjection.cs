using Bookly.Application.Common.Behaviors;
using Bookly.Application.Common.Mappings;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookly.Application.DependancyInjection;

public static class DependencyInjection
{
    public static void AddApplicaiton(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(applicationAssembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(applicationAssembly);
        TypeAdapterConfig.GlobalSettings.Scan(applicationAssembly);
        MappingExtensions.Configure(configuration["AppSettings:BaseUrl"]!);

    }
}
