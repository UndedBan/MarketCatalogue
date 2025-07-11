using MarketCatalogue.Authentication.Application.Extensions;
using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Authentication.Domain.Enumerations;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.DependencyInjection.Helpers;
using MarketCatalogue.Presentation.Areas.Shops.Models.BindingModels;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;
using MarketCatalogue.Presentation.Extensions;
using MarketCatalogue.Shared.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketCatalogue.Areas.Shops.Controllers;

[Area("Shops")]
[Route("/Shops/{Action=Index}")]
[Authorize(Roles = "Market Representative")]
public class RepresentativeShopsController : Controller
{
    private readonly IShopsService _shopsService;
    private readonly UserManager<ApplicationUser> _userManager;

    public RepresentativeShopsController(IShopsService shopsService, 
        UserManager<ApplicationUser> userManager)
    {
        _shopsService = shopsService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        var pagination = new PaginationDto(
            currentPage: page,
            itemsPerPage: ConfigurationHelper.GetValue<int>("Environment:PageSize")
        );

        var signedInUser = await _userManager.GetUserAsync(User);

        if (signedInUser is not ApplicationUser)
            return BadRequest();

        var allRepresentativeShops = await _shopsService.GetAllShopsByRepresentativeId(signedInUser.Id, pagination);
        var viewModel = allRepresentativeShops.Select(shop => shop.ToIndexViewModel()).ToList();
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult CreateShop()
    {
        var viewModel = new ShopCreateViewModel();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateShop(ShopCreateBindingModel model)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = model.ToShopCreateViewModel();
            return View(viewModel);
        }
        var dto = model.ToShopCreateDto();
        dto.MarketRepresentativeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wasCreationSuccessful = await _shopsService.CreateShop(dto);
        if (wasCreationSuccessful)
            return RedirectToAction("Index");
        else
        {
            var viewModel = model.ToShopCreateViewModel();
            ModelState.AddModelError("", "Failed to create shop. Please try again.");
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditShop(int shopId)
    {
        var shop = await _shopsService.GetShopById(shopId);
        var editShopViewModel = shop.ToEditShopViewModel();
        return View(editShopViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditShop(EditShopBindingModel model)
    {
        var shopUpdateDto = model.ToEditShopDto();
        var wasUpdateSuccessful = await _shopsService.EditShop(shopUpdateDto);
        return RedirectToAction("EditShop", new { shopId = model.Id });
    }

    public async Task<IActionResult> DeleteShop(int shopId)
    {
        var wasDeletionSuccessful = await _shopsService.DeleteShopById(shopId);
        return RedirectToAction("Index");
    }
}
