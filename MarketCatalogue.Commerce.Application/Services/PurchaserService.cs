using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Authentication.Infrastructure.Data;
using MarketCatalogue.Commerce.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Services;

public class PurchaserService : IPurchaserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthenticationDbContext _authenticationDbContext;

    public PurchaserService(UserManager<ApplicationUser> userManager, AuthenticationDbContext authenticationDbContext)
    {
        _userManager = userManager;
        _authenticationDbContext = authenticationDbContext;
    }

    public async Task<bool> UpdateBalance(string userId, decimal amountToAdd)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return false;
        user.Balance += amountToAdd;
        await _authenticationDbContext.SaveChangesAsync();
        return true;
    }
}
