using DeliveryService.Core.Models;

namespace DeliveryService.Api.Contracts
{
    public class DeliveryResponse
    {
        public string Status { get; set; }
        public Data Data { get; set; }
    }
}
