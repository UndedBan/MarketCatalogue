using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketCatalogue.Commerce.Domain.Dtos.Product;
using MarketCatalogue.Commerce.Domain.Dtos.Shared;

namespace MarketCatalogue.Commerce.Domain.Dtos.Shop;

public class ShopWithProductsDto : ShopSummaryDto
{
    public PaginatedResultDto<ProductDto> Products { get; set; }
}
