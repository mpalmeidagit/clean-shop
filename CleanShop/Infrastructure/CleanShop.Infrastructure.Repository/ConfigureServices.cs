using CleanShop.Infrastructure.Data;
using CleanShop.Infrastructure.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CleanShop.Infrastructure.Repository;

public static class ConfigureServices
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureServices()
        {
            services.AddSingleton<DapperContext>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
