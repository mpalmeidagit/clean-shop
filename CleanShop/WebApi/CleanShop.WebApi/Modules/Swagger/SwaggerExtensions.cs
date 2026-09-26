
using Microsoft.OpenApi;
using System.Reflection;

namespace CleanShop.WebApi.Modules.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
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

        return services;
    }
}