using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Entities;
using MarketCatalogue.Commerce.Domain.Interfaces;
using MarketCatalogue.Commerce.Domain.ValueObjects;
using MarketCatalogue.Commerce.Infrastructure.Data;
using MarketCatalogue.Shared.Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public ShopsService(CommerceDbContext commerceDbContext, UserManager<ApplicationUser> userManager)
    {
        _commerceDbContext = commerceDbContext;
        _userManager = userManager;
    }

    public async Task<List<Shop>> GetAllShopsByRepresentativeId(string representativeId, PaginationDto paginationDto)
    {
        var shops = await _commerceDbContext.Shops
            .Where(s => s.MarketRepresentativeId == representativeId)
            .Include(s => s.Schedule)
            .Skip(paginationDto.ToSkip())
            .Take(paginationDto.ToTake())
            .ToListAsync();

        var userIds = shops.Select(s => s.MarketRepresentativeId).Distinct().ToList();

        var marketRepresentativesDictionary = await _userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);

        foreach (var shop in shops)
        {
            if (marketRepresentativesDictionary.TryGetValue(shop.MarketRepresentativeId, out var user))
                shop.MarketRepresentative = user;
            else
                shop.MarketRepresentative = null;
        }

        return shops;
    }

    public async Task<bool> EditShop(EditShopDto editShopDto)
    {
        var shop = await _commerceDbContext.Shops
            .Include(s => s.Address)
            .Include(s => s.Schedule)
            .FirstOrDefaultAsync(s => s.Id == editShopDto.Id);

        if (shop == null)
            return false;

        shop.ShopName = editShopDto.ShopName;

        if (shop.Address == null)
            shop.Address = new Address();
        shop.Address.Street = editShopDto.Address.Street;
        shop.Address.City = editShopDto.Address.City;
        shop.Address.State = editShopDto.Address.State;
        shop.Address.PostalCode = editShopDto.Address.PostalCode;
        shop.Address.Country = editShopDto.Address.Country;

        if (shop.Schedule != null && shop.Schedule.Any())
            _commerceDbContext.Schedules.RemoveRange(shop.Schedule);

        if (editShopDto.Schedule != null)
        {
            shop.Schedule = editShopDto.Schedule.Select(s => new Schedule
            {
                Day = s.Day,
                OpenTime = s.OpenTime,
                CloseTime = s.CloseTime
            }).ToList();
        }
        else
            shop.Schedule = new List<Schedule>();

        try
        {
            await _commerceDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<Shop> GetShopById(int shopId)
    {
        var shop = await _commerceDbContext.Shops
            .Where(s => s.Id == shopId)
            .Include(s => s.Schedule)
            .FirstOrDefaultAsync();

        var marketRepresentative = await _userManager.Users
            .Where(u => u.Id == shop.MarketRepresentativeId)
            .FirstOrDefaultAsync();

        shop.MarketRepresentative = marketRepresentative;

        return shop;
    }

    public async Task<bool> CreateShop(ShopCreateDto shopCreateDto)
    {
        var shop = new Shop
        {
            ShopName = shopCreateDto.ShopName,
            Address = new Address
            {
                Street = shopCreateDto.Address.Street,
                City = shopCreateDto.Address.City,
                State = shopCreateDto.Address.State,
                PostalCode = shopCreateDto.Address.PostalCode,
                Country = shopCreateDto.Address.Country,
            },
            MarketRepresentativeId = shopCreateDto.MarketRepresentativeId ?? string.Empty,
            Schedule = shopCreateDto.Schedule?.Select(s => new Schedule
            {
                Day = s.Day,
                OpenTime = s.OpenTime,
                CloseTime = s.CloseTime
            }).ToList()
        };

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
            return false;

        _commerceDbContext.Shops.Remove(shop);
        await _commerceDbContext.SaveChangesAsync();
        return true;
    }
}
