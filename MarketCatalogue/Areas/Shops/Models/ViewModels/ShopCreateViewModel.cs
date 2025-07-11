using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using System.ComponentModel.DataAnnotations;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class ShopCreateViewModel
{
    [Required]
    public string ShopName { get; set; } = string.Empty;

    [Required]
    public AddressViewModel Address { get; set; } = new();

    public List<ScheduleViewModel>? Schedule { get; set; }
    public ShopCreateViewModel(string shopName, AddressViewModel address, List<ScheduleViewModel>? schedule)
    {
        ShopName = shopName;
        Address = address;
        Schedule = schedule;
    }
    public ShopCreateViewModel() {
            Schedule = Enum
            .GetValues<DayOfWeek>()
            .Select(day => new ScheduleViewModel { Day = day })
            .ToList();
    }
}
