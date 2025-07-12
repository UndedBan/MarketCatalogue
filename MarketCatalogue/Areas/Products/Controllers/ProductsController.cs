using AutoMapper;
using MarketCatalogue.Commerce.Application.Exceptions.Product;
using MarketCatalogue.Commerce.Domain.Dtos.Product;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.Presentation.Areas.Products.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketCatalogue.Presentation.Areas.Products.Controllers;

[Area("Products")]
[Route("[area]/[controller]/[action]")]
[Authorize(Roles = "Market Representative")]
public class ProductsController : Controller
{
    private readonly IMapper _mapper;
    private readonly IProductsService _productsService;
    private readonly ILogger<ProductsController> _logger;
    public ProductsController(IMapper mapper, IProductsService productsService, ILogger<ProductsController> logger)
    {
        _mapper = mapper;
        _productsService = productsService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult CreateProduct(int shopId, string shopName)
    {
        var createProductViewModel = new ProductCreateViewModel(shopId, shopName);
        return View(createProductViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductCreateBindingModel model)
    {
        var dto = _mapper.Map<ProductCreateDto>(model);
        var productId = await _productsService.CreateProduct(dto);
        return RedirectToAction("EditProduct", new { productId });
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int productId)
    {
        try
        {
            var productDto = await _productsService.GetProductById(productId);
            var productViewModel = _mapper.Map<ProductDetailsViewModel>(productDto);
            return View(productViewModel);
        }
        catch(ProductNotFoundException ex)
        {
            _logger.LogWarning("Product with id {ProductId} was not found. {Message}", productId, ex.Message);
            return NotFound(ex);            
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(EditProductBindingModel model)
    {
        try
        {
            var editProductDto = _mapper.Map<EditProductDto>(model);
            var wasEditSuccessful = await _productsService.EditProduct(editProductDto);

            if (!wasEditSuccessful)
                throw new ProductEditFailedException($"Editing product with ID {model.Id} failed unexpectedly.");

            return RedirectToAction("EditProduct", new { productId = model.Id });
        }
        catch(ProductNotFoundException ex)
        {
            _logger.LogWarning("Product with id {ProductId} was not found. {Message}", model.Id, ex.Message);
            return NotFound(ex);
        }
        catch (ProductEditFailedException ex)
        {
            _logger.LogError("Failed to edit product with id {ProductId}. {Message}", model.Id, ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
