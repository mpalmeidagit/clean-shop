using CleanShop.Infrastructure.Data;
using CleanShop.Infrastructure.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CleanShop.Infrastructure.Repository;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<DapperContext>();
        services.AddScoped<ICustomersRepository, CustomersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}