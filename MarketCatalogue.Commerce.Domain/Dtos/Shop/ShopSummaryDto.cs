using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Shop;

public class ShopSummaryDto
{
    public int Id { get; set; }
    public string ShopName { get; set; }
    public AddressDto Address { get; set; }
    public List<ScheduleDto> Schedule { get; set; }
}
