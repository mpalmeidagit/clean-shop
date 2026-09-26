namespace CleanShop.WebApi.Modules.Cors;

/// <summary>
/// Configuração da política de CORS da API.
/// </summary>
public static class CorsExtensions
{
    private const string PolicyName = "policyApiEcommerce";

    /// <param name="builder">Builder da aplicação.</param>
    extension(IHostApplicationBuilder builder)
    {
        /// <summary>
        /// Registra a política de CORS com a origem definida em <c>Config:OriginCors</c>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Quando <c>Config:OriginCors</c> não está configurado.</exception>
        public IHostApplicationBuilder AddCorsPolicy()
        {
            var origin = builder.Configuration["Config:OriginCors"]
                ?? throw new InvalidOperationException("Configuração 'Config:OriginCors' não encontrada.");

            builder.Services.AddCors(options => options.AddPolicy(PolicyName, policy => policy
                .WithOrigins(origin)
                .AllowAnyHeader()
                .AllowAnyMethod()));

            return builder;
        }
    }

    /// <param name="app">Aplicação web.</param>
    extension(WebApplication app)
    {
        /// <summary>
        /// Aplica a política de CORS no pipeline.
        /// </summary>
        public WebApplication UseCorsPolicy()
        {
            app.UseCors(PolicyName);
            return app;
        }
    }
}
