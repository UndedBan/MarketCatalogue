using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Shop;

public class RepresentativeShopDto
{
    public int Id { get; set; }
    public required string ShopName { get; set; }
    public int? ProductCount { get; set; }
    public required AddressDto Address { get; set; }
    public string MarketRepresentativeId { get; set; } = null!;
    [NotMapped]
    public ApplicationUser MarketRepresentative { get; set; } = null!;
    public List<ScheduleDto>? Schedule { get; set; }
}
