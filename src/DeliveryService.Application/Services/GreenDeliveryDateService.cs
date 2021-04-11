using DeliveryService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryService.Application.Services
{
    public class GreenDeliveryDateService : IGreenDeliveryDateService
    {
        public async Task<bool> IsGreenDelivery(DateTime potentialDeliveryTime)
        {
            var greenDates = new List<string> { "Wednesday", "5", "15", "25" };

            return greenDates.Contains(potentialDeliveryTime.DayOfWeek.ToString()) ||
                   greenDates.Contains(potentialDeliveryTime.Day.ToString());
        }
    }
}
