namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class AddressViewModel
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set;}
}
