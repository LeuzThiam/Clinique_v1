using MaBoutique.Payments.Application.Abstractions;
using MaBoutique.Payments.Infrastructure.External;
using MaBoutique.Payments.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaBoutique.Payments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
        services.AddScoped<IStripePaymentGateway, StripePaymentGateway>();
        return services;
    }
}
