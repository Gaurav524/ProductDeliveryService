using DeliveryService.Api.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryService.Core.Interfaces;
using DeliveryService.Core.Models;
using Microsoft.Extensions.Logging;

namespace DeliveryService.Api.Handlers
{
    public class DeliveryRequestHandler : IRequestHandler<DeliveryRequest, List<DeliveryResponse>>
    {
        private readonly IWeekService _weekService;
        private readonly IGreenDeliveryDateService _greenDeliveryDateService;
        private readonly ILogger<DeliveryRequestHandler> _logger;

        public DeliveryRequestHandler(IWeekService weekService, 
            IGreenDeliveryDateService greenDeliveryDateService,
            ILogger<DeliveryRequestHandler> logger)
        {
            _weekService = weekService;
            _greenDeliveryDateService = greenDeliveryDateService;
            _logger = logger;
        }
        public async Task<List<DeliveryResponse>> Handle(DeliveryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var deliveryResponses = await GetDeliveryDetails(request);
                return deliveryResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<DeliveryResponse>> GetDeliveryDetails(DeliveryRequest deliveryRequest)
        {
            var deliveryResponses = new List<DeliveryResponse>();

            foreach (var product in deliveryRequest.Products)
            {
                var deliveryResponse = await CalculateNextDeliveryTime(product, deliveryRequest.PostalCode);
                deliveryResponses.Add(deliveryResponse);
            }
            return deliveryResponses;
        }

        public async Task<DeliveryResponse> CalculateNextDeliveryTime(Product product, string postalCode)
        {
            var deliveryStatus = "failure";
            var deliveryDetails = new List<DeliveryDetails>();
            var deliveryResponse = new DeliveryResponse();
            var data = new Data();
            var now = product.OrderTime;

            for (var i = 0; i <= 13; i++)
            {
                var potentialDeliveryTime = now.AddDays(i);

                if (product.ProductType == ProductType.Temporary)
                {
                    var currentWeek = _weekService.GetWeek(DateTime.Now);
                    var orderWeek = _weekService.GetWeek(product.OrderTime);
                    if (!orderWeek.Equals(currentWeek)) continue;
                    if (product.DeliveryDays.Contains(potentialDeliveryTime.DayOfWeek.ToString()))
                    {
                        deliveryStatus = "success";
                        var deliveryDetail = await CreateDeliveryDetails(product, postalCode, potentialDeliveryTime);
                        if (deliveryDetail == null) continue;
                        deliveryDetails.Add(deliveryDetail);
                    }
                }
                else
                {
                    if (product.DeliveryDays.Contains(potentialDeliveryTime.DayOfWeek.ToString()))
                    {
                        deliveryStatus = "success";
                        var deliveryDetail = await CreateDeliveryDetails(product, postalCode, potentialDeliveryTime);
                        if (deliveryDetail == null) continue;
                        deliveryDetails.Add(deliveryDetail);
                    }
                }
            }

            deliveryResponse.Status = deliveryStatus;

            //TODO : Check count and dont add if count zero
            var rearranged = RearrangeDeliveryDetails(deliveryDetails, product);
            data.DeliveryDetails = rearranged;
            deliveryResponse.Data = data;
            //ToDO : Add product name in response
            return deliveryResponse;
        }

        private List<DeliveryDetails> RearrangeDeliveryDetails(List<DeliveryDetails> deliveryDetails, Product product)
        {
            var arrangedDeliveryDetails = new List<DeliveryDetails>();
            var result = deliveryDetails.Where(c => c.IsGreenDelivery &&
                                                    c.DeliveryDate <= product.OrderTime.AddDays(3));
            arrangedDeliveryDetails.AddRange(result.ToList());
            var deliveryDetailsEnumerable = deliveryDetails.Where(p => arrangedDeliveryDetails.All(p2 => p2.DeliveryDate != p.DeliveryDate));
            arrangedDeliveryDetails.AddRange(deliveryDetailsEnumerable.ToList());
            return arrangedDeliveryDetails;
        }

        //Add comments for the method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="postalCode"></param>
        /// <param name="potentialDeliveryTime"></param>
        /// <returns></returns>
        private async Task<DeliveryDetails> CreateDeliveryDetails(Product product, string postalCode, DateTime potentialDeliveryTime)
        {
            var subtractResult = (potentialDeliveryTime.Date - product.OrderTime.Date).Days;

            if (!(subtractResult < product.DaysInAdvance))
            {
                return new()
                {
                    DeliveryDate = potentialDeliveryTime,
                    PostalCode = postalCode,
                    IsGreenDelivery = await _greenDeliveryDateService.IsGreenDelivery(potentialDeliveryTime)
                };
            }
            return null;
        }
    }
}
