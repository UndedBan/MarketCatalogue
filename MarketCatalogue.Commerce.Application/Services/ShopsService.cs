using AutoMapper;
using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Application.Exceptions.Shop;
using MarketCatalogue.Commerce.Domain.Dtos.Product;
using MarketCatalogue.Commerce.Domain.Dtos.Shared;
using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Enumerations;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.Commerce.Domain.ValueObjects;
using MarketCatalogue.Commerce.Infrastructure.Data;
using MarketCatalogue.Shared.Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Services;

public class ShopsService : IShopsService
{
    private readonly CommerceDbContext _commerceDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGeocodingService _geocodingService;
    private readonly IMapper _mapper;
    private readonly ILogger<ShopsService> _logger;
    private readonly IMemoryCache _cache;

    public ShopsService(CommerceDbContext commerceDbContext, UserManager<ApplicationUser> userManager,
        IGeocodingService geocodingService, IMapper mapper, ILogger<ShopsService> logger, IMemoryCache cache)
    {
        _commerceDbContext = commerceDbContext;
        _userManager = userManager;
        _geocodingService = geocodingService;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<ShopWithProductsDto?> GetShopWithProductsById(
    int shopId,
    PaginationDto paginationDto,
    string? searchName = null,
    string? searchCategory = null)
    {
        var shop = await _commerceDbContext.Shops
            .Include(s => s.Address)
            .Include(s => s.Schedule)
            .FirstOrDefaultAsync(s => s.Id == shopId);

        if (shop == null)
            return null;

        var productsQuery = _commerceDbContext.Products
            .Where(p => p.ShopId == shopId);

        if (!string.IsNullOrEmpty(searchName))
        {
            var loweredName = searchName.ToLower();
            productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(loweredName));
        }

        if (!string.IsNullOrEmpty(searchCategory))
        {
            if (Enum.TryParse<ProductCategory>(searchCategory, ignoreCase: true, out var categoryEnum))
            {
                productsQuery = productsQuery.Where(p => p.Category == categoryEnum);
            }
        }

        var totalCount = await productsQuery.CountAsync();

        var products = await productsQuery
            .Skip(paginationDto.ToSkip())
            .Take(paginationDto.ToTake())
            .ToListAsync();

        var dto = new ShopWithProductsDto
        {
            Id = shop.Id,
            ShopName = shop.ShopName,
            Address = _mapper.Map<AddressDto>(shop.Address),
            Schedule = _mapper.Map<List<ScheduleDto>>(shop.Schedule),
            Products = new PaginatedResultDto<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(products),
                CurrentPage = paginationDto.CurrentPage,
                ItemsPerPage = paginationDto.ItemsPerPage,
                TotalItems = totalCount
            }
        };

        return dto;
    }


    public async Task<PaginatedResultDto<RepresentativeShopDto>> GetAllShopsByRepresentativeId(
    string representativeId, PaginationDto paginationDto)
    {
        var query = _commerceDbContext.Shops
            .Where(s => s.MarketRepresentativeId == representativeId)
            .Include(s => s.Schedule)
            .Include(s => s.Products);

        var totalCount = await query.CountAsync();

        var shops = await query
            .Skip(paginationDto.ToSkip())
            .Take(paginationDto.ToTake())
            .ToListAsync();

        var userIds = shops.Select(s => s.MarketRepresentativeId).Distinct().ToList();

        var marketRepresentativesDictionary = await _userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);

        var shopsListDto = new List<RepresentativeShopDto>();

        foreach (var shop in shops)
        {
            if (!marketRepresentativesDictionary.TryGetValue(shop.MarketRepresentativeId, out var representative))
            {
                _logger.LogWarning(
                    "Market representative with ID '{RepId}' was not found for shop ID {ShopId}. Skipping this shop.",
                    shop.MarketRepresentativeId, shop.Id);
                continue;
            }

            shop.MarketRepresentative = representative;

            var dto = _mapper.Map<RepresentativeShopDto>(shop);
            shopsListDto.Add(dto);
        }

        var paginatedResultDto= new PaginatedResultDto<RepresentativeShopDto>
        {
            Items = shopsListDto,
            CurrentPage = paginationDto.CurrentPage,
            ItemsPerPage = paginationDto.ItemsPerPage,
            TotalItems = totalCount
        };

        return paginatedResultDto;
    }


    public async Task<PaginatedResultDto<ShopSummaryDto>> GetAllShops(PaginationDto paginationDto)
    {
        var cacheKey = $"AllShops_Page{paginationDto.CurrentPage}_Size{paginationDto.ItemsPerPage}";

        if (_cache.TryGetValue(cacheKey, out PaginatedResultDto<ShopSummaryDto>? cachedResult) 
            && cachedResult is not null)
        {
            return cachedResult;
        }

        var query = _commerceDbContext.Shops
            .Include(s => s.Address)
            .Include(s => s.Schedule);

        var totalCount = await query.CountAsync();

        var shops = await query
            .Skip(paginationDto.ToSkip())
            .Take(paginationDto.ToTake())
            .ToListAsync();

        var shopDtos = _mapper.Map<List<ShopSummaryDto>>(shops);

        var paginatedResultDto = new PaginatedResultDto<ShopSummaryDto>
        {
            Items = shopDtos,
            CurrentPage = paginationDto.CurrentPage,
            ItemsPerPage = paginationDto.ItemsPerPage,
            TotalItems = totalCount
        };

        _cache.Set(cacheKey, paginatedResultDto, TimeSpan.FromMinutes(2));

        return paginatedResultDto;
    }

    public async Task<bool> EditShop(EditShopDto editShopDto)
    {
        var shop = await GetShopEntityById(editShopDto.Id);
        if (shop is null)
            throw new ShopNotFoundException($"Shop with ID {editShopDto.Id} was not found.");

        if (shop.Schedule?.Any() == true)
            _commerceDbContext.Schedules.RemoveRange(shop.Schedule);

        _mapper.Map(editShopDto, shop);

        await UpdateShopAddress(shop, editShopDto.Address);

        UpdateShopSchedule(shop, editShopDto.Schedule);

        var result = await _commerceDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<EditShopDto> GetShopDetailsById(int shopId)
    {
        var shop = await GetShopEntityById(shopId);

        if (shop == null)
            throw new ShopNotFoundException($"Shop with ID {shopId} was not found.");

        var marketRepresentative = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == shop.MarketRepresentativeId);

        if (marketRepresentative == null)
            throw new MarketRepresentativeNotFoundException($"Market representative with ID {shop.MarketRepresentativeId} was not found.");

        shop.MarketRepresentative = marketRepresentative;

        return _mapper.Map<EditShopDto>(shop);
    }


    public async Task<bool> CreateShop(ShopCreateDto shopCreateDto)
    {
        var shop = _mapper.Map<Shop>(shopCreateDto);

        var coordinates = await _geocodingService.GetCoordinatesAsync(shopCreateDto.Address);

        if (coordinates != null)
        {
            shop.Address.Longitude = coordinates.Value.Longitude;
            shop.Address.Latitude = coordinates.Value.Latitude;
        }

        _commerceDbContext.Shops.Add(shop);
        var result = await _commerceDbContext.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> DeleteShopById(int shopId)
    {
        var shop = await _commerceDbContext.Shops
            .Include(s => s.Schedule)
            .FirstOrDefaultAsync(s => s.Id == shopId);

        if (shop == null)
            throw new ShopNotFoundException($"Shop with ID {shopId} not found.");

        _commerceDbContext.Shops.Remove(shop);
        var result = await _commerceDbContext.SaveChangesAsync();
        return result > 0;
    }


    #region Helpers
    private async Task UpdateShopAddress(Shop shop, AddressDto addressDto)
    {
        if (shop.Address == null)
            shop.Address = new Address();

        shop.Address.Street = addressDto.Street;
        shop.Address.City = addressDto.City;
        shop.Address.State = addressDto.State;
        shop.Address.PostalCode = addressDto.PostalCode;
        shop.Address.Country = addressDto.Country;

        var coordinates = await _geocodingService.GetCoordinatesAsync(addressDto);
        if (coordinates != null)
        {
            shop.Address.Longitude = coordinates.Value.Longitude;
            shop.Address.Latitude = coordinates.Value.Latitude;
        }
    }

    private void UpdateShopSchedule(Shop shop, List<ScheduleDto>? newSchedule)
    {
        shop.Schedule = newSchedule?.Select(s => new Schedule
        {
            Day = s.Day,
            OpenTime = s.OpenTime,
            CloseTime = s.CloseTime
        }).ToList() ?? new List<Schedule>();
    }

    private async Task<Shop?> GetShopEntityById(int id)
    {
        return await _commerceDbContext.Shops
            .Include(s => s.Schedule)
            .Include(s => s.Address)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    #endregion
}
