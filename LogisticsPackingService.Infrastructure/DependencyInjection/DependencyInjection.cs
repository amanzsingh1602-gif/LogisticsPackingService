using LogisticsPackingService.Application.Interfaces;
using LogisticsPackingService.Application.Services;
using LogisticsPackingService.Infrastructure.Configuration;
using LogisticsPackingService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsPackingService.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<BoxCatalogOptions>(
            configuration.GetSection("BoxCatalog"));

        services.AddSingleton<IBoxCatalogProvider, BoxCatalogProvider>();

        services.AddScoped<IPackingService, PackingService>();

        return services;
    }
}
