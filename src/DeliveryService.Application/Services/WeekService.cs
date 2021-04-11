using System;
using System.Globalization;

namespace DeliveryService.Application.Services
{
    public class WeekService
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
