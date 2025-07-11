using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.Commerce.Infrastructure.Services;
using MarketCatalogue.DependencyInjection.Helpers;
using MarketCatalogue.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Infrastructure;

public class CommerceInfrastructureModule : IModule
{
    public void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<IGeocodingService, GeocodingService>();

        services.AddHttpClient("Geocoding", httpClient =>
        {
            httpClient.BaseAddress = new Uri(ConfigurationHelper.GetValue<string>("Geocoding:BaseUrl")!);

            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        });
    }
}
