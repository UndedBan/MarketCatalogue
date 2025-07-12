using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Mappings;

public static class ShopsMapper
{
    public static RepresentativeShopDto ToRepresentativeShopsIndexDto(this Shop shop)
    {
        return new RepresentativeShopDto
        {
            Address = shop.Address.ToDto(),
            ShopName = shop.ShopName,
            Schedule = shop.Schedule?.Select(s => s.ToScheduleDto()).ToList(),
            Id = shop.Id,
            MarketRepresentative = shop.MarketRepresentative,
            MarketRepresentativeId = shop.MarketRepresentativeId,
            ProductCount = shop.Products?.Count,
        };
    }
}
