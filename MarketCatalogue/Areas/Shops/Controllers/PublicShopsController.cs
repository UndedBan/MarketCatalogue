using AutoMapper;
using MarketCatalogue.Commerce.Application.Exceptions.Shop;
using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Enumerations;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.DependencyInjection.Helpers;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;
using MarketCatalogue.Presentation.Models;
using MarketCatalogue.Shared.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarketCatalogue.Presentation.Areas.Shops.Controllers;

[Area("Shops")]
[Route("/PublicShops/{Action=Index}")]
public class PublicShopsController : Controller
{
    private readonly IMapper _mapper;
    private readonly IShopsService _shopsService;
    private readonly ILogger<PublicShopsController> _logger;

    public PublicShopsController(IMapper mapper, IShopsService shopsService, ILogger<PublicShopsController> logger)
    {
        _mapper = mapper;
        _shopsService = shopsService;
        _logger = logger;
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

    [HttpGet]
    public async Task<IActionResult> ShopPage(
    int shopId,
    [FromQuery] int page = 1,
    [FromQuery] string? searchName = null,
    [FromQuery] string? searchCategory = null)
    {
        try
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

            var shopWithPaginatedProducts = await _shopsService.GetShopWithProductsById(
                shopId, pagination, searchName, searchCategory);

            if (shopWithPaginatedProducts == null)
                throw new ShopNotFoundException($"Shop with id {shopId} was not found.");

            var viewModel = BuildShopViewModel(shopWithPaginatedProducts,
                queryParams, 
                searchName, 
                searchCategory);

            return View(viewModel);
        }
        catch (ShopNotFoundException ex)
        {
            _logger.LogWarning("Shop not found: {Message}", ex.Message);
            return NotFound(ex.Message);
        }
    }

    #region Helpers
    private ShopWithProductsPaginatedViewModel BuildShopViewModel(ShopWithProductsDto shopWithProductsDto,
        string? queryParams,
        string? searchName = null,
        string? searchCategory = null)
    {
        return new ShopWithProductsPaginatedViewModel
            {
            ShopWithProducts = _mapper.Map<ShopWithProductsViewModel>(shopWithProductsDto),
                Pagination = new PaginationViewModel
                {
                    CurrentPage = shopWithProductsDto.Products.CurrentPage,
                    LastPage = shopWithProductsDto.Products.TotalPages,
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
    }
    #endregion
}
