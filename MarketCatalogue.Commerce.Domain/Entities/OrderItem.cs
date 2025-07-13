using MarketCatalogue.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Entities;

public class OrderItem : BaseEntity<int>
{
    [Required]
    public int OrderId { get; set; }

    public Order Order { get; set; } = null!;

    [Required]
    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public int Quantity { get; set; } = 1;

    public decimal Total => Quantity * Product.Price;
}
