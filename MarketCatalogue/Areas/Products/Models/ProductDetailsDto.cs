using MarketCatalogue.Commerce.Domain.Enumerations;

namespace MarketCatalogue.Presentation.Areas.Products.Models;

public class ProductDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; }
    public int ShopId { get; set; }
    public string ShopName { get; set; }
}
