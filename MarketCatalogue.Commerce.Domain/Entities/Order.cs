using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Enumerations;
using MarketCatalogue.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Entities;

public class Order : BaseEntity<int>
{
    [Required]
    public string PurchaserId { get; set; } = null!;

    public DateTime ArrivalDate { get; set; } = DateTime.UtcNow.AddDays(1);

    public OrderStatus OrderStatus { get; set; } = OrderStatus.InProgress;

    public List<OrderItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.Total);
}
