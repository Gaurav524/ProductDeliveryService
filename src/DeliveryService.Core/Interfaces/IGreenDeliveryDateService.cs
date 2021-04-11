using System;

namespace DeliveryService.Core.Interfaces
{
    public interface IGreenDeliveryDateService
    {
        bool IsGreenDelivery(DateTime potentialDeliveryTime);
    }
}
