using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Orders;

public class UserOrderDto
{
    [Required]
    public string PurchaserId { get; set; } = null!;

    public DateTime ArrivalDate { get; set; } = DateTime.UtcNow.AddDays(1);

    public OrderStatus OrderStatus { get; set; } = OrderStatus.InProgress;

    public List<UserOrderItemDto> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.Total);
}
