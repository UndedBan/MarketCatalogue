using MarketCatalogue.Presentation.Models;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class ShopWithProductsPaginatedViewModel
{
    public ShopWithProductsViewModel ShopWithProducts { get; set; } 
    public PaginationViewModel Pagination { get; set; }
}
