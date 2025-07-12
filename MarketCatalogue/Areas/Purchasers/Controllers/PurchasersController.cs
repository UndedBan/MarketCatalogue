using AutoMapper;
using Humanizer;
using MarketCatalogue.Commerce.Domain.Dtos.Cart;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.DependencyInjection.Helpers;
using MarketCatalogue.Presentation.Areas.Purchasers.Models;
using MarketCatalogue.Presentation.Extensions;
using MarketCatalogue.Presentation.Models;
using MarketCatalogue.Shared.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketCatalogue.Presentation.Areas.Purchasers.Controllers;

[Authorize]
[Area("Purchasers")]
[Route("[area]/[controller]/[action]")]
[Authorize(Roles = "Purchaser")]
public class PurchasersController : Controller
{
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;

    public PurchasersController(IMapper mapper, ICartService cartService)
    {
        _mapper = mapper;
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> ViewCart([FromQuery]int page = 1)
    {
        var pagination = new PaginationDto(
           currentPage: page,
           itemsPerPage: ConfigurationHelper.GetValue<int>("Environment:PageSize")
       );

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var paginatedUserCart = await _cartService.GetCartByUserId(userId, pagination);
        var userCartViewModel = new ViewCartViewModel
        {
            Items = _mapper.Map<List<ViewCartItemViewModel>>(paginatedUserCart.CartItems.Items),
            Pagination = new PaginationViewModel
            {
                CurrentPage = paginatedUserCart.CartItems.CurrentPage,
                LastPage = paginatedUserCart.CartItems.TotalPages,
                Query = ""
            },
            TotalPrice = paginatedUserCart.TotalPrice
        };

        return View(userCartViewModel);
    }

    public async Task<IActionResult> UpdateQuantity(UpdateQuantityBindingModel model)
    {
        var dto = _mapper.Map<UpdateQuantityDto>(model);
        var wasUpdateSuccessful = await _cartService.UpdateQuantity(dto);
        return RedirectToAction("ViewCart");
    }

    public async Task<IActionResult> DeleteCartItem(int cartItemId)
    {
        var wasDeletionSuccesful = await _cartService.DeleteCartItem(cartItemId);
        return RedirectToAction("ViewCart");
    }
    public async Task<IActionResult> AddToCart(AddToCartBindingModel model)
    {
        var dto = _mapper.Map<AddToCartDto>(model);
        dto.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wasAddSuccessful = await _cartService.AddToCart(dto);
        return this.RedirectToPreviousPage();
    }
}
