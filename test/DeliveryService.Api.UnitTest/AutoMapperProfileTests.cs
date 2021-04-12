using AutoMapper;
using Xunit;

namespace DeliveryService.Api.UnitTest
{
    public class AutoMapperProfileTests
    {
        [Fact]
        public void AutoMapper_CorrectConfiguration_WillNotThrowException()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Converters.AutoMapperProfile>();
            });

            var mapper = config.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
