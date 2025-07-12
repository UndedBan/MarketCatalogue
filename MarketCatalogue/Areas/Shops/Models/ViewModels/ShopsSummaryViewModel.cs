using MarketCatalogue.Presentation.Models;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class ShopsSummaryViewModel
{
    public List<ShopSummaryViewModel> Shops { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
}
