using MarketCatalogue.Commerce.Domain.Dtos.Orders;
using MarketCatalogue.Commerce.Domain.Dtos.Shared;
using MarketCatalogue.Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Interfaces;

public interface IOrdersService
{
    Task<bool> PlaceOrder(string purchaserId, string purchaserEmail);
    Task<bool> CancelOrder(int orderId, string purchaserEmail);
    Task<UserOrdersDto> GetUserOrders(string purchaserId, PaginationDto paginationDto);
    Task UpdateOrderStatusJob();
}
