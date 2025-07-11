using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Interfaces;

public interface IShopsService
{
    Task<List<Shop>> GetAllShopsByRepresentativeId(string representativeId, PaginationDto paginationDto);
    Task<Shop> GetShopById(int shopId);
    Task<bool> EditShop(EditShopDto shopUpdateDto);
    Task<bool> DeleteShopById(int shopId);
    Task<bool> CreateShop(ShopCreateDto shopCreateDto);
}
