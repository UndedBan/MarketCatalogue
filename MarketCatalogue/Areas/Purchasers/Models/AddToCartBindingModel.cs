namespace MarketCatalogue.Presentation.Areas.Purchasers.Models;

public class AddToCartBindingModel
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? ApplicationUserId { get; set; }
}
