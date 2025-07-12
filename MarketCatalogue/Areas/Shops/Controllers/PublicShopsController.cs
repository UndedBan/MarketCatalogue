using AutoMapper;
using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.DependencyInjection.Helpers;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;
using MarketCatalogue.Shared.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketCatalogue.Presentation.Areas.Shops.Controllers;

[Area("Shops")]
[Route("/PublicShops/{Action=Index}")]
public class PublicShopsController : Controller
{
    private readonly IMapper _mapper;
    private readonly IShopsService _shopsService;

    public PublicShopsController(IMapper mapper, IShopsService shopsService)
    {
        _mapper = mapper;
        _shopsService = shopsService;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        var pagination = new PaginationDto(
            currentPage: page,
            itemsPerPage: ConfigurationHelper.GetValue<int>("Environment:PageSize")
        );

        var paginatedShops = await _shopsService.GetAllShops(pagination);
        var viewModel = _mapper.Map<List<ShopSummaryViewModel>>(paginatedShops);
        return View(viewModel);
    }
    public async Task<IActionResult> ShopPage(int shopId, [FromQuery] int page = 1)
    {
        var pagination = new PaginationDto(
            currentPage: page,
            itemsPerPage: ConfigurationHelper.GetValue<int>("Environment:PageSize")
        );

        var shopWithPaginatedProducts = await _shopsService.GetShopWithProductsById(shopId, pagination);
        var viewModel = _mapper.Map<ShopWithProductsViewModel>(shopWithPaginatedProducts);
        return View(viewModel);
    }
}
