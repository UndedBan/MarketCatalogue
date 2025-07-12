using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

namespace MarketCatalogue.Presentation.Extensions;

public static class ShopMapper
{
    public static RepresentativeShopsViewModel ToIndexViewModel(this Shop shop)
    {
        return new RepresentativeShopsViewModel
        {
            Id = shop.Id,
            ShopName = shop.ShopName,
            ProductCount = shop.Products?.Count,
            Address = shop.Address,
            MarketRepresentative = shop.MarketRepresentative,
            MarketRepresentativeId = shop.MarketRepresentativeId,
            Schedule = shop.Schedule,
        };
    }

    public static EditShopViewModel ToEditShopViewModel(this Shop shop)
    {
        return new EditShopViewModel
        {
            Id = shop.Id,
            MarketRepresentative = shop.MarketRepresentative,
            ShopName = shop.ShopName,
            Address = new AddressViewModel
            {
                Street = shop.Address.Street,
                City = shop.Address.City,
                State = shop.Address.State,
                PostalCode = shop.Address.PostalCode,
                Country = shop.Address.Country,
                Longitude = shop.Address.Longitude,
                Latitude = shop.Address.Latitude,
            },
            Schedule = shop.Schedule?
                .Select(s => new ScheduleViewModel
                {
                    Day = s.Day,
                    OpenTime = s.OpenTime,
                    CloseTime = s.CloseTime
                }).ToList()
        };
    }
}
