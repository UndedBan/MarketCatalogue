using AutoMapper;
using MarketCatalogue.Commerce.Domain.Dtos.Cart;
using MarketCatalogue.Commerce.Domain.Dtos.Product;
using MarketCatalogue.Commerce.Domain.Dtos.Shared;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.Commerce.Infrastructure.Data;
using MarketCatalogue.Shared.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Services;

public class CartService : ICartService
{
    private readonly IMapper _mapper;
    private readonly CommerceDbContext _commerceDbContext;

    public CartService(IMapper mapper, CommerceDbContext commerceDbContext)
    {
        _mapper = mapper;
        _commerceDbContext = commerceDbContext;
    }

    public async Task<bool> AddToCart(AddToCartDto addToCartDto)
    {
        var userCart = await _commerceDbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.ApplicationUserId == addToCartDto.ApplicationUserId);

        if (userCart == null)
        {
            userCart = new Cart
            {
                ApplicationUserId = addToCartDto.ApplicationUserId,
                Items = new List<CartItem>()
            };

            await _commerceDbContext.Carts.AddAsync(userCart);
        }

        var existingItem = userCart.Items.FirstOrDefault(i => i.ProductId == addToCartDto.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += addToCartDto.Quantity;
        }
        else
        {
            var cartItemToAdd = _mapper.Map<CartItem>(addToCartDto);
            userCart.Items.Add(cartItemToAdd);
        }

        var result = await _commerceDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<ViewCartDto> GetCartByUserId(string userId, PaginationDto paginationDto)
    {
        var cart = await _commerceDbContext.Carts
            .Where(c => c.ApplicationUserId ==  userId)
            .FirstOrDefaultAsync();

        if (cart is not Cart)
            return null;

        var cartItemsQuery = _commerceDbContext.CartItems
            .Where(ci => ci.CartId == cart.Id);

        var totalCount = await cartItemsQuery.CountAsync();


        var cartItems = await cartItemsQuery
            .Include(ci => ci.Product)
            .Skip(paginationDto.ToSkip())
            .Take(paginationDto.ToTake())
            .ToListAsync();

        var totalPrice = await cartItemsQuery
            .SumAsync(ci => ci.Quantity * (decimal)ci.Product.Price);

        var dto = new ViewCartDto()
        {
            CartId = cart.Id,
            ApplicationUserId = cart.ApplicationUserId,
            CartItems = new PaginatedResultDto<ViewCartItemDto>
            {
                Items = _mapper.Map<List<ViewCartItemDto>>(cartItems),
                CurrentPage = paginationDto.CurrentPage,
                ItemsPerPage = paginationDto.ItemsPerPage,
                TotalItems = totalCount
            },
            TotalPrice = totalPrice,
        };

        return dto;
    }

    public async Task<bool> UpdateQuantity(UpdateQuantityDto quantityDto)
    {
        var cartItem = await _commerceDbContext.CartItems
            .Where(ci => ci.Id == quantityDto.CartItemId)
            .FirstOrDefaultAsync();

        cartItem.Quantity = quantityDto.Quantity;

        var result = await _commerceDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteCartItem(int cartItemId)
    {
        var cartItem = await _commerceDbContext.CartItems
            .Where(ci => ci.Id == cartItemId)
            .FirstOrDefaultAsync();

        var result = _commerceDbContext.Remove(cartItem);
        await _commerceDbContext.SaveChangesAsync();

        return true;
    }
}
