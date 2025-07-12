using MarketCatalogue.Commerce.Domain.Dtos.Shop;
using MarketCatalogue.Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Commerce.Application.Mappings;

public static class ScheduleMapper
{
    public static ScheduleDto ToScheduleDto(this Schedule schedule)
    {
        return new ScheduleDto
        {
            Day = schedule.Day,
            OpenTime = schedule.OpenTime,
            CloseTime = schedule.CloseTime,
        };
    }

    public static Schedule ToScheduleEntity(this ScheduleDto schedule)
    {
        return new Schedule
        {
            Day = schedule.Day,
            OpenTime = schedule.OpenTime,
            CloseTime = schedule.CloseTime,
        };
    }
}
