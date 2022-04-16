using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTeleport.Exercise.Api.Controllers;
using CTeleport.Exercise.Api.Tests.MockedObjects;
using CTeleport.Exercise.Application.Endpoints.Airports.Queires;
using CTeleport.Exercise.Application.Endpoints.Airports.Response;
using CTeleport.Exercise.Application.Interfaces.UseCase;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CTeleport.Exercise.Api.Tests.Controllers
{
    public class AirportsControllerTest
    {
        private readonly Mock<IUseCase<AirportsDistanceQuery, AirportDistanceResponse>> _mockUseCase;
        private readonly ILogger<AirportsController> _logger;

        public AirportsControllerTest()
        {
            _logger = Mock.Of<ILogger<AirportsController>>();
            _mockUseCase = new Mock<IUseCase<AirportsDistanceQuery, AirportDistanceResponse>>();
        }

        [Fact]
        public async Task Should_get_response_successfully()
        {
            //Arrange
            var request = new AirportsDistanceQuery
            {
                Destiny = "GRU",
                Origin = "CWB"
            };
            _mockUseCase.Setup(x => x.ExecuteAsync(It.IsAny<AirportsDistanceQuery>())).ReturnsAsync(MockedAirportDistanceResponse.GetResponseOk);
            var controller = new AirportsController(_mockUseCase.Object, _logger);

            //Act
            var result = await controller.GetAirpotsDistance(request);
            var response = result as OkObjectResult;

            //Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeAssignableTo<AirportDistanceResponse>();
        }

        [Fact]
        public async Task Should_give_error_due_to_error_in_use_case_iinjection()
        {
            //Arrange
            var request = new AirportsDistanceQuery
            {
                Destiny = "GRU",
                Origin = "CWB"
            };
            var controller = new AirportsController(null, _logger);

            //Act
            var result = await controller.GetAirpotsDistance(request);
            var response = result as ObjectResult;

            //Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(500);
        }
    }
}
