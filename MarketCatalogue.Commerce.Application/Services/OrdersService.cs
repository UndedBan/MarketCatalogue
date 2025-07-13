using AutoMapper;
using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Application.Exceptions.Orders;
using MarketCatalogue.Commerce.Domain.Dtos.Orders;
using MarketCatalogue.Commerce.Domain.Dtos.Product;
using MarketCatalogue.Commerce.Domain.Dtos.Shared;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Enumerations;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.Commerce.Infrastructure.Data;
using MarketCatalogue.Commerce.Infrastructure.Data.Migrations;
using MarketCatalogue.Shared.Domain.Dtos;
using MarketCatalogue.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Services;

public class OrdersService : IOrdersService
{
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;
    private readonly IProductsService _productsService;
    private readonly IPurchaserService _purchaserService;
    private readonly ISMTPCommunicatorService _smtpCommunicatorService;
    private readonly CommerceDbContext _commerceDbContext;

    public OrdersService(IMapper mapper, ICartService cartService,
        CommerceDbContext commerceDbContext,
        IProductsService productsService, IPurchaserService purchaserService, ISMTPCommunicatorService smtpCommunicatorService)
    {
        _mapper = mapper;
        _cartService = cartService;
        _commerceDbContext = commerceDbContext;
        _productsService = productsService;
        _purchaserService = purchaserService;
        _smtpCommunicatorService = smtpCommunicatorService;
    }
    public async Task<UserOrdersDto> GetUserOrders(string userId, PaginationDto paginationDto)
    {
        var ordersQuery = _commerceDbContext.Orders
            .Where(o => o.PurchaserId == userId)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product);

        var totalCount = await ordersQuery.CountAsync();

        var orders = await ordersQuery
            .OrderBy(o => o.Id)
            .Skip(paginationDto.ToSkip())
            .Take(paginationDto.ToTake())
            .ToListAsync();

        var dto = new UserOrdersDto
        {
            Orders = new PaginatedResultDto<UserOrderDto>
            {
                Items = _mapper.Map<List<UserOrderDto>>(orders),
                CurrentPage = paginationDto.CurrentPage,
                ItemsPerPage = paginationDto.ItemsPerPage,
                TotalItems = totalCount,
            }
        };

        return dto;
    }

    public async Task UpdateOrderStatusJob()
    {
        var orders = await _commerceDbContext.Orders.ToListAsync();
        foreach(var order in orders)
        {
            if (order.ArrivalDate <= DateTime.UtcNow
                && order.OrderStatus == OrderStatus.InProgress)
                order.OrderStatus = OrderStatus.Delivered;
        }
        await _commerceDbContext.SaveChangesAsync();
    }

    public async Task<bool> PlaceOrder(string purchaserId, string purchaserEmail)
    {
        var userCart = await _cartService.GetCartWithItemsByUserId(purchaserId);

        if (userCart is null)
            return false;

        var orderItems = _mapper.Map<List<OrderItem>>(userCart.Items);
        var order = new Order
        {
            PurchaserId = purchaserId,
            OrderStatus = OrderStatus.InProgress,
            ArrivalDate = DateTime.UtcNow.AddDays(1),
            Items = orderItems
        };

        _commerceDbContext.Orders.Add(order);
        await _commerceDbContext.SaveChangesAsync();

        await _cartService.ClearUserCartById(userCart.Id);

        await AdjustProductQuantity(orderItems, -1);

        await _purchaserService.UpdateBalance(purchaserId, -(order.Total));

        var message = $@"
            Thank you for your purchase! Your order (ID: {order.Id}) is currently being processed.
            Expected arrival date: {order.ArrivalDate.ToShortDateString()}.

            Order summary:
            {string.Join(Environment.NewLine, order.Items.Select(i => $"- {i.Product.Name} x{i.Quantity}"))}

            Your account has been charged {order.Total.ToString("C")}.

            We appreciate your business and look forward to serving you again!
            ";

        await _smtpCommunicatorService.SendPurchaseConfirmationEmail("Purchase Confirmation", purchaserEmail, message);

        return true;
    }

    public async Task<bool> CancelOrder(int orderId, string purchaserEmail)
    {
        var order = await _commerceDbContext.Orders
            .Where(o => o.Id == orderId)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync();

        if (order is null)
            throw new OrderNotFoundException("Order was not found.");

        order.OrderStatus = OrderStatus.Cancelled;
        var result = await _commerceDbContext.SaveChangesAsync();

        await _purchaserService.UpdateBalance(order.PurchaserId, order.Total);
        await AdjustProductQuantity(order.Items, +1);

        var message = $@"
            We regret to inform you that your order (ID: {order.Id}) has been cancelled.

            If you have already been charged, the amount of {order.Total.ToString("C")} has been refunded to your account.

            If you have any questions or need further assistance, please contact our support team.

            Thank you for understanding, and we hope to serve you in the future.
            ";

        await _smtpCommunicatorService.SendPurchaseCancellationEmail("Order Cancellation Notice", purchaserEmail, message);

        return result > 0;
    }

    #region Helpers
    private async Task AdjustProductQuantity(List<OrderItem> orderItems, int deltaMultiplier)
    {
        var productsList = new List<EditProductDto>();
        foreach (var item in orderItems)
        {
            var editedProduct = new EditProductDto
            {
                Id = item.Product.Id,
                Quantity = item.Product.Quantity + deltaMultiplier * item.Quantity
            };
            productsList.Add(editedProduct);
        }
        await _productsService.EditProductsQuantityBatch(productsList);
    }
    #endregion
}
