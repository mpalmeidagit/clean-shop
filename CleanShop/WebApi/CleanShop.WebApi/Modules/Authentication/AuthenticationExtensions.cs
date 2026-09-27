using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleanShop.WebApi.Modules.Authentication;

/// <summary>
/// 
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // JWT Configuration
        var jwtSettings = configuration.GetSection("Jwt");
        var key = jwtSettings["Key"]
            ?? throw new InvalidOperationException("Configuração 'Jwt:Key' não encontrada. Defina Jwt__Key no arquivo .env ou como variável de ambiente.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}