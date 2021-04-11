using DeliveryService.Core.Models;
using System.Collections.Generic;
using MediatR;

namespace DeliveryService.Api.Contracts
{
    public class DeliveryRequest : IRequest<DeliveryResponse>
    {
        public string PostalCode { get; set; }
        public List<Product> Products { get; set; }
    }
}
