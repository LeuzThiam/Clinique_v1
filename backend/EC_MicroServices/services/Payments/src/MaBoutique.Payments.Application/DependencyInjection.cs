using MaBoutique.Payments.Application.Abstractions;
using MaBoutique.Payments.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace MaBoutique.Payments.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentsApplication(this IServiceCollection services)
    {
        services.AddScoped<IPaymentsApplicationService, PaymentsApplicationService>();
        return services;
    }
}
