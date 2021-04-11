using System.Collections.Generic;

namespace DeliveryService.Infrastructure.Services
{
    public class GreenDeliveryInfo
    {
        public List<string> GreenDeliveryDates()
        {
            return new() { "Wednesday", "5", "15", "25" };
        }
    }
}
