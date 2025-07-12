using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Interfaces;

public interface IGeocodingService
{
    Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(AddressDto addressDto);
}
