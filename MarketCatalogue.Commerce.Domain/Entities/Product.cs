using MarketCatalogue.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketCatalogue.Commerce.Domain.Enumerations;

namespace MarketCatalogue.Commerce.Domain.Entities;

public class Product : BaseEntity<int>
{
    [Required]
    public required string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; } = ProductCategory.Other;
    public int ShopId { get; set; }
    public Shop Shop { get; set; } = null!;

    public Product() { }
}
