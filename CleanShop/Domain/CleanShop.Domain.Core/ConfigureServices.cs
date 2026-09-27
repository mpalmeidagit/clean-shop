using CleanShop.Domain.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CleanShop.Domain.Core;

public static class ConfigureServices
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDomainServices()
        {
            services.AddScoped<ICustomersDomain, CustomersDomain>();
            services.AddScoped<IUsersDomain, UsersDomain>();

            return services;
        }
    }
}
