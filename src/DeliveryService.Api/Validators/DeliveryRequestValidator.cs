using DeliveryService.Api.Contracts;
using FluentValidation;
using System.Linq;

namespace DeliveryService.Api.Validators
{
    public sealed class DeliveryRequestValidator : AbstractValidator<DeliveryRequest>
    {
        public DeliveryRequestValidator()
        {
            RuleFor(x => x.PostalCode).NotEmpty().NotNull();
            RuleFor(x => x.Products.Select(c => c.Name)).NotEmpty().NotNull();
        }
    }
}
