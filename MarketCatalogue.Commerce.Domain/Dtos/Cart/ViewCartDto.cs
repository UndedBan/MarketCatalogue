using MarketCatalogue.Commerce.Domain.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Cart;

public class ViewCartDto
{
    public PaginatedResultDto<ViewCartItemDto> CartItems { get; set; }
    public int CartId { get; set; }
    public string ApplicationUserId { get; set; }
    public decimal TotalPrice { get; set; }
}
