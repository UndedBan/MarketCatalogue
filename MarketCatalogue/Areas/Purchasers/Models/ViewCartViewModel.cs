using MarketCatalogue.Presentation.Models;

namespace MarketCatalogue.Presentation.Areas.Purchasers.Models;

public class ViewCartViewModel
{
    public List<ViewCartItemViewModel> Items { get; set; }
    public PaginationViewModel Pagination { get; set; } 
    public decimal TotalPrice { get; set; }
}
