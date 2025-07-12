using MarketCatalogue.Commerce.Domain.Enumerations;
using MarketCatalogue.Presentation.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class ShopWithProductsPaginatedViewModel
{
    public ShopWithProductsViewModel ShopWithProducts { get; set; } 
    public PaginationViewModel Pagination { get; set; }
    public List<SelectListItem>? ProductCategories { get; set; }
    public string? SearchName { get; set; }
    public string? SearchCategory { get; set; }
}
