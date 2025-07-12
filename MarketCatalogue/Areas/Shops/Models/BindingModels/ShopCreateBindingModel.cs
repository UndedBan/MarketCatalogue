using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.BindingModels;

public class ShopCreateBindingModel
{
    public string ShopName { get; set; } = string.Empty;

    public AddressBindingModel Address { get; set; } = new();

    public List<ScheduleBindingModel>? Schedule { get; set; }
}
