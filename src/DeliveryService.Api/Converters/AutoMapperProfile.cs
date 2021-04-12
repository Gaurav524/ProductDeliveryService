using AutoMapper;
using DeliveryService.Api.Contracts;
using DeliveryService.Core.Models;

namespace DeliveryService.Api.Converters
{
    public sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DeliveryRequest, DeliveryRequestSpecification>();
        }
    }
}
