using DeliveryService.Api.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductDeliveryController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly ILogger<ProductDeliveryController> _logger;

        public ProductDeliveryController(ISender mediator, 
            ILogger<ProductDeliveryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("DeliveryDates")]
        public async Task<ActionResult<List<DeliveryResponse>>> GetDeliveryDates(
            [FromBody] DeliveryRequest deliveryRequest, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(deliveryRequest, cancellationToken);
                //HttpContext.Response.StatusCode = response.Status;
                return response;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex.ToString());
            }

            return new List<DeliveryResponse>();
        }
    }
}
