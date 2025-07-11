using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.DependencyInjection.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Infrastructure.Services;

public class GeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
    public GeocodingService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Geocoding");
    }
    public async Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(AddressDto addressDto)
    {
        var apiKey = ConfigurationHelper.GetValue<string>("Geocoding:ApiKey");

        var parts = new[]
        {
            addressDto.Street,
            addressDto.City,
            addressDto.State,
            addressDto.PostalCode,
            addressDto.Country
        };

        var address = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

        var url = $"search?q={Uri.EscapeDataString(address)}&api_key={Uri.EscapeDataString(apiKey)}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();

        var results = JsonSerializer.Deserialize<List<GeocodeResponse>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var first = results?.FirstOrDefault();
        if (first == null) return null;

        return (double.Parse(first.Lat), double.Parse(first.Lon));
    }

    private class GeocodeResponse
    {
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string DisplayName { get; set; }
    }
}
