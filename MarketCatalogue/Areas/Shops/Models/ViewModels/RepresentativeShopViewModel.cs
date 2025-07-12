using MarketCatalogue.Authentication.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class RepresentativeShopViewModel
{
    public int Id { get; set; }
    public required string ShopName { get; set; }
    public int? ProductCount { get; set; }
    public required AddressViewModel Address { get; set; }
    public string MarketRepresentativeId { get; set; } = null!;
    [NotMapped]
    public ApplicationUser MarketRepresentative { get; set; } = null!;
    public List<ScheduleViewModel>? Schedule { get; set; }
}
