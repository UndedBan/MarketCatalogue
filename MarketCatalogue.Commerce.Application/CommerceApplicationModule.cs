using MarketCatalogue.Commerce.Application.Services;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MarketCatalogue.Commerce.Application;

public class CommerceApplicationModule : IModule
{
    public void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<IShopsService, ShopsService>();
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<ICartService, CartService>();
    }
}
