using MarketCatalogue.Presentation.Models;

namespace MarketCatalogue.Presentation.Areas.Purchasers.Models;

public class UserOrdersViewModel
{
    public List<UserOrderViewModel> Items { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
    public decimal UserBalance { get; set; }
}
