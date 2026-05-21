using MaBoutique.Auth.Application.Abstractions;
using MaBoutique.Auth.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace MaBoutique.Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthApplicationService, AuthApplicationService>();
        return services;
    }
}
