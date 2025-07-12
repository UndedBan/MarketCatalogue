using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Entities;

public class Cart : BaseEntity<int>
{
    public string ApplicationUserId { get; set; }
    [NotMapped]
    public ApplicationUser ApplicationUser { get; set; }
    public List<CartItem> Items { get; set; } = new();
}
