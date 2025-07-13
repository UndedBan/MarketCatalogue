using MarketCatalogue.Commerce.Domain.Dtos.Shared;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Orders;

public class UserOrdersDto
{
    public PaginatedResultDto<UserOrderDto> Orders { get; set; } = new();
}
