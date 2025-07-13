using MarketCatalogue.Commerce.Domain.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Interfaces;

public interface IProductsService
{
    Task<int> CreateProduct(ProductCreateDto productCreateDto);
    Task<bool> EditProduct(EditProductDto productEditDto);
    Task<ProductDetailsDto> GetProductById(int productId);
    Task<bool> EditProductsQuantityBatch(List<EditProductDto> productsEditDto);
}
