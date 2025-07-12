using MarketCatalogue.Commerce.Domain.Enumerations;

namespace MarketCatalogue.Presentation.Areas.Products.Models;

public class ProductCreateViewModel
{
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; }
    public int ShopId { get; set; }
    public string ShopName { get; set; }
    public ProductCreateViewModel(int shopId, string shopName)
    {
        ShopId = shopId;
        ShopName = shopName;
    }
}
