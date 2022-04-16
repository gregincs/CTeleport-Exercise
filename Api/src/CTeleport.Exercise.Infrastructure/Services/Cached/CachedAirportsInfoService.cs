using CTeleport.Exercise.Application.Interfaces.Cache;
using CTeleport.Exercise.Application.Interfaces.Services;
using CTeleport.Exercise.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CTeleport.Exercise.Infrastructure.Services.Cached
{
    public class CachedAirportsInfoService : ICachedAirportsInfoService
    {
        private readonly ILogger<CachedAirportsInfoService> _logger;
        private readonly IAirportsInfoService _airportInfoService;
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheTimeInHours;

        public CachedAirportsInfoService(ILogger<CachedAirportsInfoService> logger, IAirportsInfoService airportInfoService, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _logger = logger;
            _airportInfoService = airportInfoService;
            _memoryCache = memoryCache;
            _cacheTimeInHours = int.Parse(configuration["AirportInfoCacheTimeHours"]);
        }

        public async Task<AirportInfo> GetCachedAirportInfo(string airport)
        {
            _logger.LogInformation("Getting airport info from cache");
            if (!_memoryCache.TryGetValue(airport, out AirportInfo airportInfo))
            {
                _logger.LogInformation($"Cache not created yet to airport {airport}");
                airportInfo = await _airportInfoService.GetAirportInfoAsync(airport);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(_cacheTimeInHours));

                _memoryCache.Set(airport, airportInfo, cacheEntryOptions);
                _logger.LogInformation("Cache created successfully");
            }

            return airportInfo;
        }
    }
}
