using AutoMapper;
using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Application.Exceptions;
using MarketCatalogue.Commerce.Application.Exceptions.CartItem;
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

        userCart ??= CreateUserCart(addToCartDto.ApplicationUserId);

        var existingItem = userCart.Items
            .FirstOrDefault(i => i.ProductId == addToCartDto.ProductId);

        if (existingItem != null)
            existingItem.Quantity += addToCartDto.Quantity;
        else
        {
            var cartItemToAdd = _mapper.Map<CartItem>(addToCartDto);
            userCart.Items.Add(cartItemToAdd);
        }

        var result = await _commerceDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<ViewCartDto?> GetCartByUserId(string userId, PaginationDto paginationDto)
    {
        var cart = await _commerceDbContext.Carts
            .Where(c => c.ApplicationUserId ==  userId)
            .FirstOrDefaultAsync();

        cart ??= CreateUserCart(userId);

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

        if (cartItem is null)
            throw new CartItemDoesNotExistException("Cart item does not exist.");

        cartItem.Quantity = quantityDto.Quantity;

        var result = await _commerceDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteCartItem(int cartItemId)
    {
        var cartItem = await _commerceDbContext.CartItems
            .Where(ci => ci.Id == cartItemId)
            .FirstOrDefaultAsync();

        if (cartItem is null)
            throw new CartItemDoesNotExistException("Cart item does not exist.");

        var result = _commerceDbContext.Remove(cartItem);
        await _commerceDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<Cart?> GetCartWithItemsByUserId(string userId)
    {
        var cart = await _commerceDbContext.Carts
            .Where(c => c.ApplicationUserId == userId)
            .Include(c => c.Items)
                .ThenInclude(c => c.Product)
            .FirstOrDefaultAsync();

        return cart;
    }

    public async Task<bool> ClearUserCartById(int cartId)
    {
        var cart = await _commerceDbContext.Carts
            .Where(c => c.Id == cartId)
                .Include(c => c.Items)
            .FirstOrDefaultAsync();

        if (cart is null)
            throw new UserCartNullException("User cart was not found.");

        _commerceDbContext.Carts.Remove(cart);
        return await _commerceDbContext.SaveChangesAsync() > 0;
    }
    private Cart CreateUserCart(string applicationUserId)
    {
        var userCart = new Cart
        {
            ApplicationUserId = applicationUserId,
            Items = new List<CartItem>()
        };

        _commerceDbContext.Carts.Add(userCart);
        return userCart;
    }
}
