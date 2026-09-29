using CleanShop.Application.Validator;

namespace CleanShop.WebApi.Modules.Validator;

/// <summary>
/// 
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddValidator(this IServiceCollection services)
    {
        services.AddTransient<SignInDtoValidator>();
        services.AddTransient<SignUpDtoValidator>();
        return services;
    }
}