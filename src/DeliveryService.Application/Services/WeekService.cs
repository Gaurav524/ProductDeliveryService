using System;
using System.Globalization;
using DeliveryService.Core.Interfaces;

namespace DeliveryService.Application.Services
{
    public class WeekService :IWeekService
    {
        public int GetWeek(DateTime orderTime)
        {
            var cultureInfo = CultureInfo.CurrentCulture;
            var weekNum = cultureInfo.Calendar.GetWeekOfYear(orderTime,
                CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            return weekNum;
        }
    }
}
