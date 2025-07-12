using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Cart;

public class UpdateQuantityDto
{
    public int CartItemId { get; set; }
    public int Quantity { get; set; }
}
