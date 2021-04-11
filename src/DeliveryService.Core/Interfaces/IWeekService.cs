using System;

namespace DeliveryService.Core.Interfaces
{
    public interface IWeekService
    {
        int GetWeek(DateTime orderTime);
    }
}
