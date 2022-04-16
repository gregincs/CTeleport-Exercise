using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTeleport.Exercise.Application.Endpoints.Airports.Queires;
using CTeleport.Exercise.Application.Interfaces.Cache;
using CTeleport.Exercise.Application.Tests.MockedObjects;
using CTeleport.Exercise.Application.UseCases;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CTeleport.Exercise.Application.Tests.UseCase
{
    public class AirportsDistanceUseCaseTests
    {
        private readonly ILogger<AirportsDistanceUseCase> _logger;
        private readonly Mock<ICachedAirportsInfoService> _mockCachedAirportsInfoService;

        public AirportsDistanceUseCaseTests()
        {
            _logger = Mock.Of<ILogger<AirportsDistanceUseCase>>();
            _mockCachedAirportsInfoService = new Mock<ICachedAirportsInfoService>();
        }

        [Fact]
        public async Task Should_get_airport_distance_response_successfully()
        {
            //Arrange
            _mockCachedAirportsInfoService.Setup(x => x.GetCachedAirportInfo("GRU")).ReturnsAsync(MockedAirportInfo.GetGruOk());
            _mockCachedAirportsInfoService.Setup(x => x.GetCachedAirportInfo("CWB")).ReturnsAsync(MockedAirportInfo.GetCwbOk());
            var useCase = new AirportsDistanceUseCase(_logger, _mockCachedAirportsInfoService.Object);
            var request = new AirportsDistanceQuery
            {
                Destiny = "CWB",
                Origin = "GRU"
            };

            //Act
            var result = await useCase.ExecuteAsync(request);

            //Assert
            result.Should().NotBeNull();
            result.DestinyAirport.City.Should().Be("Curitiba");
            result.DestinyAirport.Country.Should().Be("Brazil");
            result.DestinyAirport.Iata.Should().Be("CWB");

            result.OriginAirport.City.Should().Be("Sao Paulo");
            result.OriginAirport.Country.Should().Be("Brazil");
            result.OriginAirport.Iata.Should().Be("GRU");

            result.DistanceInKilometers.Should().Be("359,78 Km");
        }

        [Fact]
        public async Task Should_throw_null_refence_exception_due_to_null_interface()
        {
            //Arrange
            var useCase = new AirportsDistanceUseCase(_logger, null);
            var request = new AirportsDistanceQuery
            {
                Destiny = "CWB",
                Origin = "GRU"
            };

            //Act
            Func<Task> act = async () => await useCase.ExecuteAsync(request);

            //Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error trying to execute Airport Info UseCase: Object reference not set to an instance of an object.");
        }
    }
}
