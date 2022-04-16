using System.Net;
using CTeleport.Exercise.Infrastructure.Services;
using CTeleport.Exercise.Infrastructure.Services.HttpClients;
using CTeleport.Exercise.Infrastructure.Tests.MockedObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;
using Xunit;

namespace CTeleport.Exercise.Infrastructure.Tests.Service
{
    public class AirportsInfoServiceTests
    {
        private readonly ILogger<AirportsInfoService> _logger;
        private readonly Mock<IAirportInfoClient> _mockApiClient;

        public AirportsInfoServiceTests()
        {
            _logger = Mock.Of<ILogger<AirportsInfoService>>();
            _mockApiClient = new Mock<IAirportInfoClient>();
        }

        [Fact]
        public async Task Should_get_airport_info_successfully()
        {
            //Arrange
            _mockApiClient.Setup(x => x.GetAirportInfoByIataCodeAsync(It.IsAny<string>())).ReturnsAsync(MockedAirportInfo.GetGruOk());
            var service = new AirportsInfoService(_logger, _mockApiClient.Object);

            //Act
            var result = await service.GetAirportInfoAsync("GRU");

            //Assert
            result.Should().NotBeNull();
            result.City.Should().Be("Sao Paulo");
            result.Iata.Should().Be("GRU");
            result.CityIata.Should().Be("SAO");
            result.Country.Should().Be("Brazil");
            result.Location.Lat.Should().Be(-23.425668);
            result.Location.Lon.Should().Be(-46.481926);
        }

        [Fact]
        public async Task Should_not_find_airport_()
        {
            //Arrange
            var refitSettings = new RefitSettings();
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            var apiException = ApiException.Create(null, HttpMethod.Get, httpResponseMessage, refitSettings);

            _mockApiClient.Setup(x => x.GetAirportInfoByIataCodeAsync(It.IsAny<string>())).Throws(await apiException);

            var service = new AirportsInfoService(_logger, _mockApiClient.Object);

            //Act
            var result = await service.GetAirportInfoAsync("gru");

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_give_internal_server_error_trying_to_get_airport()
        {
            //Arrange
            var refitSettings = new RefitSettings();
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            var apiException = ApiException.Create(null, HttpMethod.Get, httpResponseMessage, refitSettings);

            _mockApiClient.Setup(x => x.GetAirportInfoByIataCodeAsync(It.IsAny<string>())).Throws(await apiException);

            var service = new AirportsInfoService(_logger, _mockApiClient.Object);

            //Act
            Func<Task> act = async () => await service.GetAirportInfoAsync("GRU");

            //Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("ApiException trying to reach AirportInfo api: Response status code does not indicate success: 500 (Internal Server Error).");
        }

        [Fact]
        public async Task Should_give_null_reference_exception_due_to_failing_injecting_refit_interface()
        {
            //Arrange
            var service = new AirportsInfoService(_logger, null);

            //Act
            Func<Task> act = async () => await service.GetAirportInfoAsync("GRU");

            //Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error trying to execute AirportService: Object reference not set to an instance of an object.");
        }
    }
}
