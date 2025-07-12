using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.BindingModels;

public class ShopCreateBindingModel
{
    public string ShopName { get; set; } = string.Empty;

    public AddressBindingModel Address { get; set; } = new();

    public List<ScheduleBindingModel>? Schedule { get; set; }

    public ShopCreateDto ToShopCreateDto()
    {
        return new ShopCreateDto
        {
            ShopName = this.ShopName,
            Address = new AddressDto
            {
                Street = this.Address.Street,
                City = this.Address.City,
                State = this.Address.State,
                PostalCode = this.Address.PostalCode,
                Country = this.Address.Country
            },
            Schedule = this.Schedule?
                .Select(s => new ScheduleDto
                {
                    Day = s.Day,
                    OpenTime = s.OpenTime,
                    CloseTime = s.CloseTime
                }).ToList()
        };
    }

    public ShopCreateViewModel ToShopCreateViewModel()
    {
        return new ShopCreateViewModel
        {
            ShopName = this.ShopName,
            Address = new AddressViewModel
            {
                Street = this.Address.Street,
                City = this.Address.City,
                State = this.Address.State,
                PostalCode = this.Address.PostalCode,
                Country = this.Address.Country
            },
            Schedule = this.Schedule?
                .Select(s => new ScheduleViewModel
                {
                    Day = s.Day,
                    OpenTime = s.OpenTime,
                    CloseTime = s.CloseTime
                }).ToList()
        };
    }
}
