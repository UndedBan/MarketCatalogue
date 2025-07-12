using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class RepresentativeShopsViewModel
{
    public int Id { get; set; }
    public required string ShopName { get; set; }
    public int? ProductCount { get; set; }
    public required Address Address { get; set; }
    public string MarketRepresentativeId { get; set; } = null!;
    [NotMapped]
    public ApplicationUser MarketRepresentative { get; set; } = null!;
    public List<Schedule>? Schedule { get; set; }
}
