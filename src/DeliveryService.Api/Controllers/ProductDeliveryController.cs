using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryService.Api.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeliveryService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductDeliveryController : ControllerBase
    {
        private readonly ILogger<ProductDeliveryController> _logger;

        public ProductDeliveryController(ILogger<ProductDeliveryController> logger)
        {
            _logger = logger;
        }

        [HttpPost("DeliveryDates")]
        public async Task<ActionResult<DeliveryResponse>> GetDeliveryDates(
            [FromBody] DeliveryRequest deliveryRequest, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
