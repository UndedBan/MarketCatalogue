using AutoMapper;
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
    public ProductsController(IMapper mapper, IProductsService productsService)
    {
        _mapper = mapper;
        _productsService = productsService;
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
        var productDto = await _productsService.GetProductById(productId);
        var productViewModel = _mapper.Map<ProductDetailsViewModel>(productDto);
        return View(productViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(EditProductBindingModel model)
    {
        var editProductDto = _mapper.Map<EditProductDto>(model);
        var wasEditSuccessful = await _productsService.EditProduct(editProductDto);
        return RedirectToAction("EditProduct", new { productId = model.Id });
    }
}
