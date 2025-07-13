using AutoMapper;
using AutoMapper.QueryableExtensions;
using MarketCatalogue.Commerce.Application.Exceptions;
using MarketCatalogue.Commerce.Application.Exceptions.Product;
using MarketCatalogue.Commerce.Domain.Dtos.Product;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Services;

public class ProductsService : IProductsService
{
    private readonly IMapper _mapper;
    private readonly CommerceDbContext _commerceDbContext;
    private readonly IMemoryCache _cache;
    public ProductsService(IMapper mapper, CommerceDbContext commerceDbContext, IMemoryCache cache)
    {
        _mapper = mapper;
        _commerceDbContext = commerceDbContext;
        _cache = cache;
    }

    public async Task<int> CreateProduct(ProductCreateDto productCreateDto)
    {
        var product = _mapper.Map<Product>(productCreateDto);

        _commerceDbContext.Products.Add(product);
        await _commerceDbContext.SaveChangesAsync();

        return product.Id;
    }

    public async Task<bool> EditProduct(EditProductDto productEditDto)
    {
        var product = await _commerceDbContext.Products
            .FirstOrDefaultAsync(p => p.Id == productEditDto.Id);

        if (product == null)
            throw new ProductNotFoundException("Edit product failed. ");

        product.Name = productEditDto.Name;
        product.Quantity = productEditDto.Quantity;
        product.Price = productEditDto.Price;
        product.Category = productEditDto.Category;

        _commerceDbContext.Products.Update(product);
        var result = await _commerceDbContext.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> EditProductsQuantityBatch(List<EditProductDto> productsEditDto)
    {
        var productIds = productsEditDto.Select(p => p.Id).ToList();
        var products = await _commerceDbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count == 0)
            throw new ProductNotFoundException("Edit product failed. No matching products found.");

        foreach (var product in products)
        {
            var dto = productsEditDto.FirstOrDefault(pe => pe.Id == product.Id);
            if (dto == null)
                continue;

            if (dto.Quantity >= 0)
                product.Quantity = dto.Quantity;
        }

        var result = await _commerceDbContext.SaveChangesAsync();
        return result > 0;
    }


    public async Task<ProductDetailsDto> GetProductById(int productId)
    {
        var product = await _commerceDbContext.Products
            .Where(p => p.Id == productId)
            .ProjectTo<ProductDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (product == null)
            throw new ProductNotFoundException("Product not found.");

        return product;
    }
}
