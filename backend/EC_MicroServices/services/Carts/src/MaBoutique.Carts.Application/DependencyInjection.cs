using MaBoutique.Carts.Application.Abstractions;
using MaBoutique.Carts.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace MaBoutique.Carts.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCartsApplication(this IServiceCollection services)
    {
        services.AddScoped<ICartsApplicationService, CartsApplicationService>();
        return services;
    }
}
