using MaBoutique.Carts.Application.Abstractions;
using MaBoutique.Carts.Infrastructure.Persistence;
using MaBoutique.Carts.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaBoutique.Carts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCartsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICartRepository, EfCartRepository>();

        return services;
    }
}
