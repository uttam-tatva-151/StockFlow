using StockFlow.Common.Constants;
using StockFlow.Repository.Contexts;
using StockFlow.Repository.Implementations;
using StockFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StockFlow.Repository.Extensions;
public static class DependencyInjection
{
    public static void RegisterDBContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString(Constant.AppDbConnection));
        });
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserAuthRepository, UserAuthRepository>();
    }
}
