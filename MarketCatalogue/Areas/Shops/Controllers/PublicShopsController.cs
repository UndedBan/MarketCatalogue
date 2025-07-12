using AutoMapper;
using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Enumerations;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.DependencyInjection.Helpers;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;
using MarketCatalogue.Presentation.Models;
using MarketCatalogue.Shared.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        var viewModel = new ShopsSummaryViewModel
        {
            Shops = _mapper.Map<List<ShopSummaryViewModel>>(paginatedShops.Items),
            Pagination = new PaginationViewModel
            {
                CurrentPage = paginatedShops.CurrentPage,
                LastPage = paginatedShops.TotalPages,
                Query = ""
            }
        };

        return View(viewModel);
    }
    public async Task<IActionResult> ShopPage(int shopId, [FromQuery] int page = 1, [FromQuery] string? searchName = null,
    [FromQuery] string? searchCategory = null)
    {
        var pagination = new PaginationDto(
            currentPage: page,
            itemsPerPage: ConfigurationHelper.GetValue<int>("Environment:PageSize")
        );

        var queryParams = $"shopId={shopId}";

        if (!string.IsNullOrEmpty(searchName))
            queryParams += $"&searchName={Uri.EscapeDataString(searchName)}";

        if (!string.IsNullOrEmpty(searchCategory))
            queryParams += $"&searchCategory={Uri.EscapeDataString(searchCategory)}";

        var shopWithPaginatedProducts = await _shopsService.GetShopWithProductsById(shopId, pagination, searchName, searchCategory);
        var viewModel = new ShopWithProductsPaginatedViewModel
        {
            ShopWithProducts = _mapper.Map<ShopWithProductsViewModel>(shopWithPaginatedProducts),
            Pagination = new PaginationViewModel
            {
                CurrentPage = shopWithPaginatedProducts.Products.CurrentPage,
                LastPage = shopWithPaginatedProducts.Products.TotalPages,
                Query = queryParams
            },
            ProductCategories = Enum.GetValues(typeof(ProductCategory))
                .Cast<ProductCategory>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                }).ToList(),
            SearchName = searchName,
            SearchCategory = searchCategory
        };
        return View(viewModel);
    }
}
