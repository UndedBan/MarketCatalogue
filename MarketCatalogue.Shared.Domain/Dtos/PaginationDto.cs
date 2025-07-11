namespace MarketCatalogue.Shared.Domain.Dtos;

public class PaginationDto
{
    public int CurrentPage { get; }
    public int Pages { get; }
    public int ItemsPerPage { get; }

    public PaginationDto(int? currentPage, int pages = 1, int itemsPerPage = 10)
    {
        if (currentPage == null || currentPage < 1)
            currentPage = 1;

        CurrentPage = (int)currentPage;

        if (pages < 1)
            pages = 1;

        Pages = pages;

        if (itemsPerPage < 1)
            itemsPerPage = 10;

        ItemsPerPage = itemsPerPage;
    }

    public int ToSkip()
    {
        return (CurrentPage - 1) * ItemsPerPage;
    }

    public int ToTake()
    {
        return Pages * ItemsPerPage;
    }
}
