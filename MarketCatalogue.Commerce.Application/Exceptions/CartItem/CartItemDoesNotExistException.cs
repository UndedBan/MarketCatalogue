﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Exceptions.CartItem;

public class CartItemDoesNotExistException(string message) : Exception(message) { }