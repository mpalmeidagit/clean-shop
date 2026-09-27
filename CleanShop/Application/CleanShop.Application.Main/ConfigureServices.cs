using CleanShop.Application.Interface;
using CleanShop.Transversal.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanShop.Application.Main;

public static class ConfigureServices
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationServices()
        {
            services.AddScoped<ICustomersApplication, CustomersApplication>();
            services.AddScoped<IAuthApplication, AuthApplication>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
