using System;
using System.Threading.Tasks;

namespace DeliveryService.Core.Interfaces
{
    public interface IGreenDeliveryDateService
    {
        Task<bool> IsGreenDelivery(DateTime potentialDeliveryTime);
    }
}
