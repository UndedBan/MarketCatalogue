using MarketCatalogue.Commerce.Domain.Enumerations;

namespace MarketCatalogue.Presentation.Areas.Products.Models;

public class EditProductBindingModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; }
    public EditProductBindingModel(int id, string name, int quantity, decimal price, ProductCategory category)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Price = price;
        Category = category;
    }

    public EditProductBindingModel() { }
}
