using MarketCatalogue.Commerce.Domain.Enumerations;

namespace MarketCatalogue.Presentation.Areas.Products.Models;

public class ProductCreateBindingModel{

    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; }
    public int ShopId { get; set; }
    public ProductCreateBindingModel(string name, int quantity, decimal price, ProductCategory category, int shopId)
    {
        Name = name;
        Quantity = quantity;
        Price = price;
        Category = category;
        ShopId = shopId;
    }
    public ProductCreateBindingModel() { }
}
