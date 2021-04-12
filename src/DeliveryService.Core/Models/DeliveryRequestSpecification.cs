using System.Collections.Generic;

namespace DeliveryService.Core.Models
{
    public sealed class DeliveryRequestSpecification
    {
        public string PostalCode { get; set; }
        public List<Product> Products { get; set; }
    }
}
