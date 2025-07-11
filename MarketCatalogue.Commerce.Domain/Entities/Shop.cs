using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Domain.ValueObjects;
using MarketCatalogue.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Entities;

public class Shop : BaseEntity<int>
{
    [Required]
    public required string ShopName { get; set; }
    public List<Product>? Products { get; set; }
    [Required]
    public required Address Address { get; set; }
    [Required]
    public string MarketRepresentativeId { get; set; } = null!;
    [NotMapped]
    public ApplicationUser MarketRepresentative { get; set; } = null!;
    public List<Schedule>? Schedule { get; set; }
    public Shop() { }
}
