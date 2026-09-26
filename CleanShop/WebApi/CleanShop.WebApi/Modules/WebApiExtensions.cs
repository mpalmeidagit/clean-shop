using CleanShop.Application.Main;
using CleanShop.Domain.Core;
using CleanShop.Infrastructure.Repository;
using CleanShop.WebApi.Modules.Cors;
using CleanShop.WebApi.Modules.Swagger;

namespace CleanShop.WebApi.Modules;

/// <summary>
/// Ponto único de composição da Web API: serviços e pipeline HTTP.
/// </summary>
public static class WebApiExtensions
{
    /// <param name="builder">Builder da aplicação.</param>
    extension(IHostApplicationBuilder builder)
    {
        /// <summary>
        /// Registra os serviços da API e das camadas Domain, Infrastructure e Application.
        /// </summary>
        public IHostApplicationBuilder AddWebApi()
        {
            builder.Services.AddControllers();

            builder.Services.AddDomainServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.AddApplicationServices();

            builder.AddSwagger();
            builder.AddCorsPolicy();

            return builder;
        }
    }

    /// <param name="app">Aplicação web.</param>
    extension(WebApplication app)
    {
        /// <summary>
        /// Configura o pipeline HTTP. A ordem dos middlewares importa.
        /// </summary>
        public WebApplication UseWebApi()
        {
            app.UseSwaggerDocumentation();
            app.UseHttpsRedirection();
            app.UseCorsPolicy();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
