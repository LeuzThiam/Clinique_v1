using MaBoutique.Auth.Application.Abstractions;
using MaBoutique.Auth.Infrastructure.External;
using MaBoutique.Auth.Infrastructure.Options;
using MaBoutique.Auth.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MaBoutique.Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<UsersClientOptions>(configuration.GetSection(UsersClientOptions.SectionName));

        services.AddHttpClient<IUsersClient, UsersHttpClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<UsersClientOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
        });

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
}
