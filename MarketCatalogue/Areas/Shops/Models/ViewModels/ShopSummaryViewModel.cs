using MarketCatalogue.Commerce.Domain.Dtos.Shop;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class ShopSummaryViewModel
{
    public int Id { get; set; }
    public string ShopName { get; set; }
    public AddressViewModel Address { get; set; }
    public List<ScheduleViewModel> Schedule { get; set; }
}
