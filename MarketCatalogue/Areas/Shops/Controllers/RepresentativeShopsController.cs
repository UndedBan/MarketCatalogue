using AutoMapper;
using ClosedXML.Excel;
using MarketCatalogue.Authentication.Application.Extensions;
using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Authentication.Domain.Enumerations;
using MarketCatalogue.Commerce.Application.Exceptions.Shop;
using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.DependencyInjection.Helpers;
using MarketCatalogue.Presentation.Areas.Shops.Models.BindingModels;
using MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;
using MarketCatalogue.Presentation.Exceptions;
using MarketCatalogue.Presentation.Models;
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
    private readonly IMapper _mapper;
    private readonly ILogger<RepresentativeShopsController> _logger;
    private readonly IProductsService _productsService;

    public RepresentativeShopsController(IShopsService shopsService,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        ILogger<RepresentativeShopsController> logger,
        IProductsService productsService)
    {
        _shopsService = shopsService;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _productsService = productsService;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        try
        {
            var pagination = new PaginationDto(
                currentPage: page,
                itemsPerPage: ConfigurationHelper.GetValue<int>("Environment:PageSize")
            );

            var signedInUser = await _userManager.GetUserAsync(User);

            if (signedInUser is null)
                throw new UserWasNotFoundException("User was not found or not authenticated.");

            var paginatedShopsDto = await _shopsService.GetAllShopsByRepresentativeId(signedInUser.Id, pagination);

            var viewModel = new RepresentativeShopsViewModel
            {
                Shops = _mapper.Map<List<RepresentativeShopViewModel>>(paginatedShopsDto.Items),
                Pagination = new PaginationViewModel
                {
                    CurrentPage = paginatedShopsDto.CurrentPage,
                    LastPage = paginatedShopsDto.TotalPages,
                    Query = ""
                }
            };

            return View(viewModel);
        }
        catch (UserWasNotFoundException ex)
        {
            _logger.LogError(ex, "Error loading shops for the representative. User was not found.");
            return BadRequest("Error");
        }
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
            var viewModel = _mapper.Map<ShopCreateViewModel>(model);
            return View(viewModel);
        }

        var dto = _mapper.Map<ShopCreateDto>(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            throw new UserWasNotFoundException("User was not found or not authenticated.");

        dto.MarketRepresentativeId = userId;

        try
        {
            await _shopsService.CreateShop(dto);
            return RedirectToAction("Index");
        }
        catch (ShopCreationFailedException ex)
        {
            _logger.LogError(ex, "Failed to create shop for user {UserId}", userId);
            ModelState.AddModelError("", "Failed to create shop. Please try again.");
            var viewModel = _mapper.Map<ShopCreateViewModel>(model);
            return View(viewModel);
        }
        catch (UserWasNotFoundException ex)
        {
            _logger.LogWarning(ex, "User not found during shop creation.");
            ModelState.AddModelError("", "You must be logged in to create a shop.");
            var viewModel = _mapper.Map<ShopCreateViewModel>(model);
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditShop(int shopId)
    {
        try
        {
            var shop = await _shopsService.GetShopDetailsById(shopId);
            var editShopViewModel = _mapper.Map<EditShopViewModel>(shop);
            return View(editShopViewModel);
        }
        catch (MarketRepresentativeNotFoundException ex)
        {
            _logger.LogWarning("Market representative for shop ID {ShopId} not found. {Message}", shopId, ex.Message);
            return NotFound(ex.Message);
        }
        catch (ShopNotFoundException ex)
        {
            _logger.LogWarning("Shop with ID {ShopId} not found. {Message}", shopId, ex.Message);
            return NotFound(ex.Message);
        }
    }


    [HttpPost]
    public async Task<IActionResult> EditShop(EditShopBindingModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("EditShop: Invalid model state for shop ID {ShopId}", model.Id);
                var viewModel = _mapper.Map<EditShopViewModel>(model);
                return View(viewModel);
            }

            var shopUpdateDto = _mapper.Map<EditShopDto>(model);
            var wasUpdateSuccessful = await _shopsService.EditShop(shopUpdateDto);

            if (!wasUpdateSuccessful)
            {
                _logger.LogWarning("EditShop: Update failed for shop ID {ShopId}", model.Id);
                ModelState.AddModelError(string.Empty, "Update failed. Please try again.");
                return View(model);
            }

            return RedirectToAction("EditShop", new { shopId = model.Id });
        }
        catch (ShopNotFoundException ex)
        {
            _logger.LogWarning("EditShop: Shop with ID {ShopId} not found. {Message}", model.Id, ex.Message);
            return NotFound(ex.Message);
        }
    }


    [HttpPost]
    public async Task<IActionResult> DeleteShop(int shopId)
    {
        try
        {
            var wasDeleted = await _shopsService.DeleteShopById(shopId);

            _logger.LogInformation("Shop with ID {ShopId} deleted successfully.", shopId);
            return RedirectToAction("Index");
        }
        catch (ShopNotFoundException ex)
        {
            _logger.LogWarning("Deletion failed: {Message}", ex.Message);
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetProductsReport()
    {
        var representativeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var products = await _productsService.GetAllProductsByMarketRepresentativeId(representativeId);

        using var workbook = new XLWorkbook();

        var groupedByShop = products.GroupBy(p => p.Shop.ShopName);

        foreach (var shopGroup in groupedByShop)
        {
            var worksheet = workbook.Worksheets.Add(shopGroup.Key);

            worksheet.Cell(1, 1).Value = "Product Name";
            worksheet.Cell(1, 2).Value = "Quantity";
            worksheet.Cell(1, 3).Value = "Price";
            worksheet.Cell(1, 4).Value = "Category";

            int row = 2;
            foreach (var product in shopGroup)
            {
                worksheet.Cell(row, 1).Value = product.Name;
                worksheet.Cell(row, 2).Value = product.Quantity;
                worksheet.Cell(row, 3).Value = product.Price;
                worksheet.Cell(row, 4).Value = product.Category.ToString();
                row++;
            }

            worksheet.Columns().AdjustToContents();
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        var fileName = $"ProductsReport_{DateTime.UtcNow:yyyyMMdd_HHmm}.xlsx";
        return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
    }
}
