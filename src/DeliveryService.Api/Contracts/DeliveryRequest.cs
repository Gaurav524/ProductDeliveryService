using DeliveryService.Core.Models;
using System.Collections.Generic;

namespace DeliveryService.Api.Contracts
{
    public class DeliveryRequest
    {
        public string PostalCode { get; set; }
        public List<Product> Products { get; set; }
    }
}
