using CleanShop.Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanShop.Application.Main;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomersApplication, CustomersApplication>();
        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        return services;
    }
}