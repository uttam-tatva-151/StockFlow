using StockFlow.Repository.Extensions;
using StockFlow.Service.Implementations;
using StockFlow.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace StockFlow.Service.Extensions;
public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterRepositories();
        //Registering Services
        services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
        services.AddScoped<IUserAuthService, UserAuthService>();
        services.AddScoped<IJWTService, JWTService>();

    }
}
