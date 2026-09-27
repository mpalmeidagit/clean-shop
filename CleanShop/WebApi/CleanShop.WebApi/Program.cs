using CleanShop.WebApi.Modules;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddWebApi();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseWebApi();

try
{
    Log.Information("Iniciando CleanShop.Ecommerce API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicativo encerrado inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}
