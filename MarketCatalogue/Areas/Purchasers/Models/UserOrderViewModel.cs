namespace MarketCatalogue.Presentation.Areas.Purchasers.Models;

public class UserOrderViewModel
{
    public string PurchaserId { get; set; } = null!;
    public DateTime ArrivalDate { get; set; }
    public string OrderStatus { get; set; } = null!;
    public List<UserOrderItemViewModel> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Total);
}
