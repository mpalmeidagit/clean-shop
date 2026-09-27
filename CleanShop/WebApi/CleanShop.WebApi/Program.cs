using CleanShop.WebApi.Modules;
using Serilog;

// Carrega os segredos do arquivo .env (local, fora do Git) como variáveis de ambiente.
// NoClobber: variáveis já definidas (Docker, servidor) têm prioridade sobre o arquivo.
DotNetEnv.Env.NoClobber().TraversePath().Load();

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
