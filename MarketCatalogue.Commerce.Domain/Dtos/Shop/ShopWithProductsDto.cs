using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Shop;

public class ShopWithProductsDto : ShopSummaryDto
{
    public List<ProductDto> Products { get; set; }
}
