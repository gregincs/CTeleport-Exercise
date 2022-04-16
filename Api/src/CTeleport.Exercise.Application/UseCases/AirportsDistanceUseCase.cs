using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTeleport.Exercise.Application.Endpoints.Airports.Queires;
using CTeleport.Exercise.Application.Endpoints.Airports.Response;
using CTeleport.Exercise.Application.Interfaces.Cache;
using CTeleport.Exercise.Application.Interfaces.UseCase;
using GeoCoordinatePortable;
using Microsoft.Extensions.Logging;

namespace CTeleport.Exercise.Application.UseCases
{
    public class AirportsDistanceUseCase : IUseCase<AirportsDistanceQuery, AirportDistanceResponse>
    {
        private readonly ILogger<AirportsDistanceUseCase> _logger;
        private readonly ICachedAirportsInfoService _cachedAirportsInfoService;

        public AirportsDistanceUseCase(ILogger<AirportsDistanceUseCase> logger, ICachedAirportsInfoService cachedAirportsInfoService)
        {
            _logger = logger;
            _cachedAirportsInfoService = cachedAirportsInfoService;
        }

        public async Task<AirportDistanceResponse> ExecuteAsync(AirportsDistanceQuery request)
        {
            try
            {
                var airportOriginInfoTask = _cachedAirportsInfoService.GetCachedAirportInfo(request.Origin);
                var airportDestinyInfoTask = _cachedAirportsInfoService.GetCachedAirportInfo(request.Destiny);

                var airportOriginInfo = await airportOriginInfoTask;
                var airportDestinyInfo = await airportDestinyInfoTask;

                if (airportOriginInfo == null)
                {
                    _logger.LogError($"Origin airport {request.Origin} not found");
                    throw new Exception($"Origin airport {request.Origin} not found");
                }

                if (airportDestinyInfo == null)
                {
                    _logger.LogError($"Destiny airport {request.Destiny} not found");
                    throw new Exception($"Destiny airport {request.Destiny} not found");
                }

                var originCoordinate = new GeoCoordinate(airportOriginInfo.Location.Lat, airportOriginInfo.Location.Lon);
                var destinyCoordinate = new GeoCoordinate(airportDestinyInfo.Location.Lat, airportDestinyInfo.Location.Lon);

                var distance = Math.Round(originCoordinate.GetDistanceTo(destinyCoordinate)/1000, 2); // in kilometer with 2 decimal places

                var response = new AirportDistanceResponse
                {
                    DestinyAirport = airportDestinyInfo,
                    DistanceInKilometers = $"{distance} Km",
                    OriginAirport = airportOriginInfo
                };

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error trying to execute Airport Info UseCase: {e.Message}");
                throw new Exception($"Error trying to execute Airport Info UseCase: {e.Message}");
            }
        }
    }
}
