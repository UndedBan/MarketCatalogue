namespace MarketCatalogue.Presentation.Areas.Purchasers.Models;

public class ViewCartItemViewModel
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int AvailableStock { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Price => Quantity * UnitPrice;
}
