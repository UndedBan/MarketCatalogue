namespace MarketCatalogue.Presentation.Areas.Shops.Models.BindingModels;

public class ScheduleBindingModel
{
    public DayOfWeek Day { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
}
