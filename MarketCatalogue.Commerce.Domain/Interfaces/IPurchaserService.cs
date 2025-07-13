using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Interfaces;

public interface IPurchaserService
{
    Task<bool> UpdateBalance(string userId, decimal amountToAdd);
}
