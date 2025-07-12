using MarketCatalogue.Commerce.Domain.Dtos.Cart;
using MarketCatalogue.Commerce.Domain.Dtos.Shared;
using MarketCatalogue.Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Interfaces;

public interface ICartService
{
    Task<bool> AddToCart(AddToCartDto addToCartDto);
    Task<ViewCartDto?> GetCartByUserId(string userId, PaginationDto paginationDto);
    Task<bool> UpdateQuantity(UpdateQuantityDto quantityDto);
    Task<bool> DeleteCartItem(int cartItemId);
}
