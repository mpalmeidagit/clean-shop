using CleanShop.Domain.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CleanShop.Core;

public static class ConfigureServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomersDomain, CustomersDomain>();
        return services;
    }
}