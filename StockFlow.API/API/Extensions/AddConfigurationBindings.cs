using Microsoft.VisualBasic;
using StockFlow.Common.Constants;
using StockFlow.Common.Models;

namespace StockFlow.API.Extensions
{
    /// <summary>
    /// Provides extension methods for binding configuration settings to strongly-typed objects.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Binds configuration sections to strongly-typed objects used in the application.
        /// </summary>
        /// <param name="services">The IServiceCollection instance to add configuration bindings to.</param>
        /// <param name="configuration">The IConfiguration instance to retrieve configuration sections from.</param>
        /// <returns>The updated IServiceCollection instance.</returns>
        /// <remarks>
        /// This method binds the following configuration sections:
        /// - JWT settings (JwtConfig)
        /// - Email settings (EmailSettings)
        /// - Route settings (RouteSettings)
        /// Ensure that these sections are properly defined in the appsettings.json file.
        /// </remarks>
        /// 
        public static IServiceCollection AddConfigurationBindings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(Constant.AppSettings));
            return services;
        }
    }
}
