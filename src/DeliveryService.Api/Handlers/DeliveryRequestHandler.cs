using DeliveryService.Api.Contracts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Api.Handlers
{
    public class DeliveryRequestHandler : IRequestHandler<DeliveryRequest, DeliveryResponse>
    {
        public Task<DeliveryResponse> Handle(DeliveryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
