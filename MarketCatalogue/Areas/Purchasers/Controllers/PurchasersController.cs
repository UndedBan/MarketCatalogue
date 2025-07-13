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
using MarketCatalogue.Presentation.Exceptions;
using MarketCatalogue.Commerce.Application.Exceptions.CartItem;
using MarketCatalogue.Commerce.Application.Exceptions.Shared;
using MarketCatalogue.Commerce.Application.Exceptions.Orders;
using Microsoft.AspNetCore.Identity;
using MarketCatalogue.Authentication.Domain.Entities;

namespace MarketCatalogue.Presentation.Areas.Purchasers.Controllers;

[Authorize]
[Area("Purchasers")]
[Route("[area]/[controller]/[action]")]
[Authorize(Roles = "Purchaser")]
public class PurchasersController : Controller
{
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;
    private readonly IOrdersService _ordersService;
    private readonly UserManager<ApplicationUser> _userManager;
    private ILogger<PurchasersController> _logger;
    public PurchasersController(IMapper mapper, ICartService cartService, ILogger<PurchasersController> logger, IOrdersService ordersService, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _cartService = cartService;
        _logger = logger;
        _ordersService = ordersService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> ViewCart([FromQuery]int page = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            var pagination = new PaginationDto(
               currentPage: page,
               itemsPerPage: ConfigurationHelper.GetValue<int>("Environment:PageSize")
           );

            if (userId is null)
                throw new UserWasNotFoundException("No user id was found");

            var paginatedUserCart = await _cartService.GetCartByUserId(userId, pagination);

            if (paginatedUserCart == null)
                throw new UserCartNullException("No cart was found.");

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
        catch(UserWasNotFoundException ex)
        {
            _logger.LogError("No user id was found during Cart retrieval. {msg}", ex.Message);
            return NotFound(ex);
        }
        catch(UserCartNullException ex)
        {
            _logger.LogError("User with id {id} has no cart. {msg}", userId, ex.Message);
            return BadRequest(ex);
        }
    }

    public async Task<IActionResult> PlaceOrder()
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            if (user is null)
                throw new UserWasNotFoundException("No user id was found");

            var wasOrderPlacementSuccessful = await _ordersService.PlaceOrder(user.Id, user.Email);

            return this.RedirectToPreviousPage();
        }
        catch (UserWasNotFoundException ex)
        {
            _logger.LogError("No user id was found during Cart retrieval. {msg}", ex.Message);
            return NotFound(ex);
        }
        catch (UserCartNullException ex)
        {
            _logger.LogError("User with id {id} has no cart. {msg}", user.Id, ex.Message);
            return BadRequest(ex);
        }
    }

    public async Task<IActionResult> ViewOrders([FromQuery] int page = 1)
    {
        var pagination = new PaginationDto(
            currentPage: page,
            itemsPerPage: ConfigurationHelper.GetValue<int>("Environment:PageSize")
        );

        var user = await _userManager.GetUserAsync(User);
        try
        {
            if (user is null)
                throw new UserWasNotFoundException("No user id was found");

            var paginatedUserOrders = await _ordersService.GetUserOrders(user.Id, pagination);

            var userOrdersViewModel = new UserOrdersViewModel
            {
                Items = _mapper.Map<List<UserOrderViewModel>>(paginatedUserOrders.Orders.Items),
                Pagination = new PaginationViewModel
                {
                    CurrentPage = paginatedUserOrders.Orders.CurrentPage,
                    LastPage = paginatedUserOrders.Orders.TotalPages,
                    Query = ""
                },
                UserBalance = user.Balance
            };

            return View(userOrdersViewModel);
        }
        catch (UserWasNotFoundException ex)
        {
            _logger.LogError("No user id was found during Cart retrieval. {msg}", ex.Message);
            return NotFound(ex);
        }
    }

    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            if (user is null)
                throw new UserWasNotFoundException("No user id was found");
            var wasCancellationSuccessful = await _ordersService.CancelOrder(orderId, user.Email);
            if (!wasCancellationSuccessful)
                throw new OrderCancellationFailedException("Order cancellation failed.");
            return this.RedirectToPreviousPage();
        }
        catch(OrderCancellationFailedException ex)
        {
            _logger.LogError("Order cancellation failed for order with id {id}. {msg}", orderId, ex.Message);
            return NotFound(ex);
        }
        catch (OrderNotFoundException ex)
        {
            _logger.LogError("No order found with id {id}. {msg}", orderId, ex.Message);
            return NotFound(ex);
        }
        catch (UserWasNotFoundException ex)
        {
            _logger.LogError("No user id was found during Cart retrieval. {msg}", ex.Message);
            return NotFound(ex);
        }
    }

    public async Task<IActionResult> UpdateQuantity(UpdateQuantityBindingModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var dto = _mapper.Map<UpdateQuantityDto>(model);
            var wasUpdateSuccessful = await _cartService.UpdateQuantity(dto);

            if (!wasUpdateSuccessful)
                throw new UpdateFailedException("Failed to update quantity in database.");

            return RedirectToAction("ViewCart");
        }
        catch(CartItemDoesNotExistException ex)
        {
            _logger.LogError("Cart Item with id {id} does not exist. {msg}", model.CartItemId, ex.Message);
            return NotFound(ex);
        }
        catch(UpdateFailedException ex)
        {
            _logger.LogError("Failed to update quantity {quantity} for Cart Item {cartItemId}. {msg}", 
                model.Quantity,
                model.CartItemId, 
                ex.Message);

            return BadRequest(ex);
        }
    }

    public async Task<IActionResult> DeleteCartItem(int cartItemId)
    {
        try
        {
            var wasDeletionSuccessful = await _cartService.DeleteCartItem(cartItemId);
            if (!wasDeletionSuccessful)
                throw new Exception($"Failed to delete cart item with id {cartItemId}.");

            return RedirectToAction("ViewCart");
        }
        catch (CartItemDoesNotExistException ex)
        {
            _logger.LogError("Delete failed - Cart item not found: {CartItemId}. {Message}", cartItemId, ex.Message);
            return NotFound(new { error = ex.Message });
        }
    }
    public async Task<IActionResult> AddToCart(AddToCartBindingModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = _mapper.Map<AddToCartDto>(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new UserWasNotFoundException("No user id was found");

            dto.ApplicationUserId = userId;

            var wasAddSuccessful = await _cartService.AddToCart(dto);
            if (!wasAddSuccessful)
                throw new AddCartItemFailException("Failed to add item to cart.");

            return this.RedirectToPreviousPage();
        }
        catch (UserWasNotFoundException ex)
        {
            _logger.LogWarning("Add to cart failed - user not found. {Message}", ex.Message);
            return NotFound(new { error = ex.Message });
        }
        catch(AddCartItemFailException ex)
        {
            _logger.LogWarning("Add to cart failed {Message}", ex.Message);
            return BadRequest(ex);
        }
    }
}
