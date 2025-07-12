using MarketCatalogue.Authentication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Shop;

public class EditShopDto
{
    public int Id { get; set; }
    [Required]
    public string ShopName { get; set; } = string.Empty;

    [Required]
    public AddressDto Address { get; set; } = new();

    public List<ScheduleDto>? Schedule { get; set; }

}
