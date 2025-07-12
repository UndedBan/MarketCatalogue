namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class ShopWithProductsViewModel
{
    public int Id { get; set; }
    public string ShopName { get; set; }
    public AddressViewModel Address { get; set; } = null!;
    public List<ScheduleViewModel> Schedule { get; set; } = new();
    public List<ProductViewModel> Products { get; set; } = new();
}
