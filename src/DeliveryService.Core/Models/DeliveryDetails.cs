using System;

namespace DeliveryService.Core.Models
{
    public sealed class DeliveryDetails
    {
        public string PostalCode { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsGreenDelivery { get; set; }
    }
}
