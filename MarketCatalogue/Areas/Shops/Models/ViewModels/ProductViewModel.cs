using MarketCatalogue.Commerce.Domain.Enumerations;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; }
}
