using DeliveryService.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace DeliveryService.Application.Services
{
    public class GreenDeliveryDateService : IGreenDeliveryDateService
    {
        public bool IsGreenDelivery(DateTime potentialDeliveryTime)
        {
            var greenDates = new List<string> { "Wednesday", "5", "15", "25" };

            return greenDates.Contains(potentialDeliveryTime.DayOfWeek.ToString()) ||
                   greenDates.Contains(potentialDeliveryTime.Day.ToString());
        }
    }
}
