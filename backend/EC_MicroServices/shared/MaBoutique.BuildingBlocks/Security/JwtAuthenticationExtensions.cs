using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MaBoutique.BuildingBlocks.Security;

public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddConfiguredJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Configuration manquante: Jwt:Key");
        var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Configuration manquante: Jwt:Issuer");
        var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Configuration manquante: Jwt:Audience");

        ValidateJwtSettings(key, issuer, audience);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

        return services;
    }

    private static void ValidateJwtSettings(string key, string issuer, string audience)
    {
        if (key.StartsWith("CHANGE_ME_", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Jwt:Key utilise encore un placeholder CHANGE_ME_. Configurez la valeur via User Secrets ou variables d'environnement.");

        if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("Jwt:Issuer et Jwt:Audience doivent etre renseignes.");
    }
}
