using MarketCatalogue.Commerce.Domain.Dtos.Product;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

namespace MarketCatalogue.Presentation.Areas.Purchasers.Models;

public class UserOrderItemViewModel
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public ProductViewModel Product { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public decimal Total => Quantity * Product.Price;
}
