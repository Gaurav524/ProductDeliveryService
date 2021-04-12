using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Application.Services;
using DeliveryService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryService.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeekService(this IServiceCollection services)
        {
            services.AddScoped<IWeekService, WeekService>();
            return services;
        }

        public static IServiceCollection AddGreenDeliveryDateService(this IServiceCollection services)
        {
            services.AddScoped<IGreenDeliveryDateService, GreenDeliveryDateService>();
            return services;
        }

        public static IServiceCollection AddAutoMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Converters.AutoMapperProfile));
            return services;
        }
    }
}
