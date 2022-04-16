using CTeleport.Exercise.Application.Interfaces.Services;
using CTeleport.Exercise.Domain.Entities;
using CTeleport.Exercise.Infrastructure.Services.Cached;
using CTeleport.Exercise.Infrastructure.Tests.MockedObjects;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CTeleport.Exercise.Infrastructure.Tests.Service.Cached
{
    public class CachedAirportsInfoServiceTests
    {
        private readonly ILogger<CachedAirportsInfoService> _logger;
        private readonly Mock<IAirportsInfoService> _mockAirportInfoService;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public CachedAirportsInfoServiceTests()
        {
            _logger = Mock.Of<ILogger<CachedAirportsInfoService>>();
            _mockAirportInfoService = new Mock<IAirportsInfoService>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x[It.Is<string>(s => s == "AirportInfoCacheTimeHours")]).Returns("24");
        }

        [Fact]
        public async Task Should_get_cache_successfully()
        {
            //Arrange
            object mockedAirportInfo = MockedAirportInfo.GetGruOk();
            _mockMemoryCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out mockedAirportInfo)).Returns(true);
            var cachedService = new CachedAirportsInfoService(_logger, _mockAirportInfoService.Object, _mockMemoryCache.Object, _mockConfiguration.Object);

            //Act
            var result = await cachedService.GetCachedAirportInfo("GRU");

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
        public async Task Should_get_reach_airport_service_then_save_airportInfo()
        {
            //Arrange
            var mockedGruInfo = MockedAirportInfo.GetGruOk();
            object mockedAirportInfo = mockedGruInfo;
            var cacheEntry = Mock.Of<ICacheEntry>();

            _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<string>())).Returns(cacheEntry);
            _mockMemoryCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out mockedAirportInfo)).Returns(false);
            _mockAirportInfoService.Setup(x => x.GetAirportInfoAsync(It.IsAny<string>())).ReturnsAsync(mockedGruInfo);

            var cachedService = new CachedAirportsInfoService(_logger, _mockAirportInfoService.Object, _mockMemoryCache.Object, _mockConfiguration.Object);

            //Act
            await cachedService.GetCachedAirportInfo("GRU");
            var cachedResult = (AirportInfo)_mockMemoryCache.Object.Get("GRU");

            //Assert
            cachedResult.Should().NotBeNull();
            cachedResult.City.Should().Be("Sao Paulo");
            cachedResult.Iata.Should().Be("GRU");
            cachedResult.CityIata.Should().Be("SAO");
            cachedResult.Country.Should().Be("Brazil");
            cachedResult.Location.Lat.Should().Be(-23.425668);
            cachedResult.Location.Lon.Should().Be(-46.481926);
        }
    }
}
