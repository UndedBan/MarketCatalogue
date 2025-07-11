using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.BindingModels;

public class EditShopBindingModel
{
    public int Id { get; set; }
    public string ShopName { get; set; } = string.Empty;
    public AddressBindingModel Address { get; set; } = new();
    public List<ScheduleBindingModel>? Schedule { get; set; }
    public EditShopBindingModel(string shopName, AddressBindingModel address, List<ScheduleBindingModel>? schedule, int id)
    {
        ShopName = shopName;
        Address = address;
        Schedule = schedule;
        Id = id;
    }
    public EditShopBindingModel() { }

    public EditShopDto ToEditShopDto()
    {
        return new EditShopDto
        {
            Id = Id,
            ShopName = ShopName,
            Address = new AddressDto
            {
                Street = Address.Street,
                City = Address.City,
                State = Address.State,
                PostalCode = Address.PostalCode,
                Country = Address.Country
            },
            Schedule = Schedule?
                .Select(s => new ScheduleDto
                {
                    Day = s.Day,
                    OpenTime = s.OpenTime,
                    CloseTime = s.CloseTime
                }).ToList()
        };
    }
}
