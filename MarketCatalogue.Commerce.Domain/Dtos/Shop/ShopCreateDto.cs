using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Shop;

public class ShopCreateDto
{
    [Required]
    public string ShopName { get; set; } = string.Empty;

    [Required]
    public AddressDto Address { get; set; } = new();

    public List<ScheduleDto>? Schedule { get; set; }

    public string? MarketRepresentativeId { get; set; }
    public ShopCreateDto(string shopName, AddressDto address, List<ScheduleDto>? schedule, string? marketRepresentativeId)
    {
        ShopName = shopName;
        Address = address;
        Schedule = schedule;
        MarketRepresentativeId = marketRepresentativeId;
    }
    public ShopCreateDto() { }
}
