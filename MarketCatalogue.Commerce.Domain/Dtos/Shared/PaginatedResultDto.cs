﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Domain.Dtos.Shared;

public class PaginatedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
}