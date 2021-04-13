using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DeliveryService.Api.Contracts;
using DeliveryService.Api.Handlers;
using DeliveryService.Core.Interfaces;
using DeliveryService.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace DeliveryService.Api.UnitTest
{
    public class DeliveryRequestHandlerTest
    {
        private DeliveryRequestHandler _sut;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IWeekService> weekServiceMock;
        private readonly Mock<IGreenDeliveryDateService> greenDeliveryDateServiceMock;
        private readonly Mock<ILogger<DeliveryRequestHandler>> loggerMock;

        public DeliveryRequestHandlerTest()
        {
            weekServiceMock = new Mock<IWeekService>();
            greenDeliveryDateServiceMock = new Mock<IGreenDeliveryDateService>();
            mapperMock = new Mock<IMapper>();
            loggerMock = new Mock<ILogger<DeliveryRequestHandler>>();
        }

        [Fact]
        public async Task Handle_DeliveryRequest_WillReturnExpectedResponse()
        {
            var deliveryRequestSpecification = DeliveryRequestSpecification_With_OneProduct_Data();

            mapperMock.Setup(x =>
                    x.Map<DeliveryRequestSpecification>(It.IsAny<DeliveryRequest>()))
                .Returns(deliveryRequestSpecification);

            _sut = new DeliveryRequestHandler(
                weekServiceMock.Object,
                greenDeliveryDateServiceMock.Object,
                mapperMock.Object,
                loggerMock.Object);

            var response = await _sut.Handle(
                DeliveryRequest_With_OneProduct_Data(), 
                default);

            response.Should().NotBeEmpty();
            response.Select(x => x.Status).Should().Equal("success");
            response.Select(x => x.Product).Should().Equal("Test");
            response.Select(x => x.Data).Should().NotBeEmpty();
            response.Select(x => x.Data.DeliveryDetails.Should().SatisfyRespectively(
                first =>
                {
                    first.PostalCode.Should().Be("12345A");
                    first.IsGreenDelivery.Should().BeFalse();
                    first.DeliveryDate.Should().Be(19.April(2021));

                },
                second =>
                {
                    second.PostalCode.Should().Be("12345A");
                    second.IsGreenDelivery.Should().BeFalse();
                    second.DeliveryDate.Should().Be(20.April(2021));
                },
                third =>
                {
                    third.PostalCode.Should().Be("12345A");
                    third.IsGreenDelivery.Should().BeFalse();
                    third.DeliveryDate.Should().Be(26.April(2021));
                }
                ));
        }

        private static DeliveryRequestSpecification DeliveryRequestSpecification_With_OneProduct_Data()
        {
            var deliveryRequestSpecification = new DeliveryRequestSpecification()
            {
                PostalCode = "12345A",
                Products = new List<Product>()
                {
                    new()
                    {
                        ProductId = "1",
                        Name = "Test",
                        DeliveryDays = new List<string>() {"Monday", "Tuesday"},
                        ProductType = 0,
                        DaysInAdvance = 1,
                        OrderTime = DateTime.Parse("2021-04-13T07:00:30.751Z")
                    }
                }
            };
            return deliveryRequestSpecification;
        }

        private static DeliveryRequest DeliveryRequest_With_OneProduct_Data()
        {
            var deliveryRequest = new DeliveryRequest()
            {
                PostalCode = "12345A",
                Products = new List<Product>()
                {
                    new()
                    {
                        ProductId = "1",
                        Name = "Test",
                        DeliveryDays = new List<string>() {"Monday", "Tuesday"},
                        ProductType = 0,
                        DaysInAdvance = 1,
                        OrderTime = DateTime.Parse("2021-04-13T07:00:30.751Z")
                    }
                }
            };
            return deliveryRequest;
        }
    }
}
