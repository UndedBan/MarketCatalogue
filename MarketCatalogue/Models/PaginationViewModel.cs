namespace MarketCatalogue.Presentation.Models;

public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int LastPage { get; set; }
    public string? Query { get; set; }
}
