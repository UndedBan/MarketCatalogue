﻿using MarketCatalogue.Presentation.Models;

namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class RepresentativeShopsViewModel
{
    public List<RepresentativeShopViewModel> Shops { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
}
