using AutoMapper;
using DeliveryService.Api.Contracts;
using DeliveryService.Api.Validators;
using DeliveryService.Core.Interfaces;
using DeliveryService.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryService.Core.Exceptions;

namespace DeliveryService.Api.Handlers
{
    public class DeliveryRequestHandler : IRequestHandler<DeliveryRequest, List<DeliveryResponse>>
    {
        private readonly IWeekService _weekService;
        private readonly IGreenDeliveryDateService _greenDeliveryDateService;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryRequestHandler> _logger;

        public DeliveryRequestHandler(IWeekService weekService, 
            IGreenDeliveryDateService greenDeliveryDateService,
            IMapper mapper,
            ILogger<DeliveryRequestHandler> logger)
        {
            _weekService = weekService ?? throw new ArgumentNullException(nameof(weekService));
            _greenDeliveryDateService = greenDeliveryDateService ?? throw new ArgumentNullException(nameof(greenDeliveryDateService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<DeliveryResponse>> Handle(DeliveryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new DeliveryRequestValidator().Validate(request);
                if (!validator.IsValid)
                {
                    _logger.LogError("Validation failed");
                    throw new ValidationException();
                }

                var deliveryRequestSpecification = _mapper.Map<DeliveryRequestSpecification>(request);

                var deliveryResponses = await GetDeliveryDetails(deliveryRequestSpecification);
                return deliveryResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        private async Task<List<DeliveryResponse>> GetDeliveryDetails(DeliveryRequestSpecification deliveryRequest)
        {
            var deliveryResponses = new List<DeliveryResponse>();

            foreach (var product in deliveryRequest.Products)
            {
                var deliveryResponse = await CalculateNextDeliveryTime(product, deliveryRequest.PostalCode);
                deliveryResponses.Add(deliveryResponse);
            }
            return deliveryResponses;
        }

        private async Task<DeliveryResponse> CalculateNextDeliveryTime(Product product, string postalCode)
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
                    if (!IsOkToOrder(product.OrderTime)) continue;

                    if (!product.DeliveryDays.Contains(potentialDeliveryTime.DayOfWeek.ToString())) continue;

                    deliveryStatus = "success";
                    var deliveryDetail = await CreateDeliveryDetails(product, postalCode, potentialDeliveryTime);
                    if (deliveryDetail == null) continue;
                    deliveryDetails.Add(deliveryDetail);
                }
                else
                {
                    if (!product.DeliveryDays.Contains(potentialDeliveryTime.DayOfWeek.ToString())) continue;

                    deliveryStatus = "success";
                    var deliveryDetail = await CreateDeliveryDetails(product, postalCode, potentialDeliveryTime);
                    if (deliveryDetail == null) continue;
                    deliveryDetails.Add(deliveryDetail);
                }
            }

            deliveryResponse.Status = deliveryStatus;

            var rearranged = RearrangeDeliveryDetailsOnPriority(deliveryDetails, product);

            data.DeliveryDetails = rearranged;
            deliveryResponse.Data = data;
            deliveryResponse.Product = product.Name;
            return deliveryResponse;
        }

        private async Task<DeliveryDetails> CreateDeliveryDetails(Product product, string postalCode, DateTime potentialDeliveryTime)
        {
            var subtractResult = (potentialDeliveryTime.Date - product.OrderTime.Date).Days;

            if (subtractResult < product.DaysInAdvance) return null;

            var details = new DeliveryDetails
            {
                DeliveryDate = potentialDeliveryTime,
                PostalCode = postalCode,
                IsGreenDelivery = await _greenDeliveryDateService.IsGreenDelivery(potentialDeliveryTime)
            };

            return details;
        }

        private List<DeliveryDetails> RearrangeDeliveryDetailsOnPriority(List<DeliveryDetails> deliveryDetails, Product product)
        {
            var arrangedDeliveryDetails = new List<DeliveryDetails>();
            var result = deliveryDetails.Where(c => c.IsGreenDelivery &&
                                                    c.DeliveryDate <= product.OrderTime.AddDays(3));

            arrangedDeliveryDetails.AddRange(result.ToList());

            var deliveryDetailsEnumerable = deliveryDetails.Where(p => 
                arrangedDeliveryDetails.All(p2 => p2.DeliveryDate != p.DeliveryDate));

            arrangedDeliveryDetails.AddRange(deliveryDetailsEnumerable.ToList());
            return arrangedDeliveryDetails;
        }

        private bool IsOkToOrder(DateTime orderTime)
        {
            var currentWeek = _weekService.GetWeek(DateTime.Now);
            var orderWeek = _weekService.GetWeek(orderTime);
            return orderWeek.Equals(currentWeek);
        }
    }
}
