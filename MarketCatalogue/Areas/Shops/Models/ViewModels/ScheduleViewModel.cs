namespace MarketCatalogue.Presentation.Areas.Shops.Models.ViewModels;

public class ScheduleViewModel
{
    public DayOfWeek Day { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
    public ScheduleViewModel() { }

    public ScheduleViewModel(DayOfWeek day, TimeSpan openTime, TimeSpan closeTime)
    {
        Day = day;
        OpenTime = openTime;
        CloseTime = closeTime;
    }
}
