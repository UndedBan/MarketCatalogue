﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Exceptions.Product;

public class ProductNotFoundException(string message) : Exception(message) { }

