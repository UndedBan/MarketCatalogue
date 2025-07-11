using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using System.ComponentModel.DataAnnotations;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class EditShopViewModel
{
    public int Id { get; set; }
    [Required]
    public string ShopName { get; set; } = string.Empty;

    [Required]
    public AddressViewModel Address { get; set; } = new();

    public List<ScheduleViewModel>? Schedule { get; set; }
    public ApplicationUser MarketRepresentative { get; set; }
    public EditShopViewModel(string shopName, AddressViewModel address, List<ScheduleViewModel>? schedule, ApplicationUser marketRepresentative, int id)
    {
        ShopName = shopName;
        Address = address;
        Schedule = schedule;
        MarketRepresentative = marketRepresentative;
        Id = id;
    }
    public EditShopViewModel() { }
}
