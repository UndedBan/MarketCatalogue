﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Cart;

public class AddToCartDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string ApplicationUserId { get; set; }
}
