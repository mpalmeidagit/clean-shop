using Microsoft.OpenApi;
using System.Reflection;

namespace CleanShop.WebApi.Modules.Swagger;

/// <summary>
/// Configuração da documentação da API com Swagger (Swashbuckle).
/// </summary>
public static class SwaggerExtensions
{
    /// <param name="builder">Builder da aplicação.</param>
    extension(IHostApplicationBuilder builder)
    {
        /// <summary>
        /// Registra o gerador do documento Swagger da API.
        /// </summary>
        public IHostApplicationBuilder AddSwagger()
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Clean Shop - Mercado de APIs de serviços de tecnologia",
                    Description = "Um simples example ASP.NET Core Web API. ",
                    TermsOfService = new Uri("https://cleanshop@com.br/termos"),
                    Contact = new OpenApiContact
                    {
                        Name = "Clean Shop",
                        Email = "cleanshop@com.br",
                        Url = new Uri("https://cleanshop@com.br/faleconosco")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Utilizar sob CLSHOP",
                        Url = new Uri("https://cleanshop@com.br/licenca")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();
            });

            return builder;
        }
    }

    /// <param name="app">Aplicação web.</param>
    extension(WebApplication app)
    {
        /// <summary>
        /// Expõe o documento Swagger e a Swagger UI (somente em Development).
        /// </summary>
        public WebApplication UseSwaggerDocumentation()
        {
            if (!app.Environment.IsDevelopment())
                return app;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = "swagger";
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.ShowExtensions();
            });

            return app;
        }
    }
}
