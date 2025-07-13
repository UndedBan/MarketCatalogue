using MarketCatalogue.Commerce.Domain.Dtos.Product;
using MarketCatalogue.Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Orders;

public class UserOrderItemDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public ProductDto Product { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public decimal Total => Quantity * Product.Price;
}
