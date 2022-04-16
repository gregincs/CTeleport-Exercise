using CTeleport.Exercise.Application.Interfaces.Services;
using CTeleport.Exercise.Domain.Entities;
using CTeleport.Exercise.Infrastructure.Services.HttpClients;
using Microsoft.Extensions.Logging;
using Refit;

namespace CTeleport.Exercise.Infrastructure.Services
{
    public class AirportsInfoService : IAirportsInfoService
    {
        private readonly ILogger<AirportsInfoService> _logger;
        private readonly IAirportInfoClient _apiClient;

        public AirportsInfoService(ILogger<AirportsInfoService> logger, IAirportInfoClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task<AirportInfo> GetAirportInfoAsync(string airportIataCode)
        {
            try
            {
                _logger.LogInformation("Trying to reach AirportInfo api");
                var response = await _apiClient.GetAirportInfoByIataCodeAsync(airportIataCode);

                _logger.LogInformation("AirportInfo api called successfully for {airport}", airportIataCode);
                return response;
            }
            catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Airport IATA Code not found");
                return null;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"ApiException trying to reach AirportInfo api: {ex.GetBaseException().Message}");
                throw new Exception($"ApiException trying to reach AirportInfo api: {ex.GetBaseException().Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error trying to execute AirportService: {ex.Message}");
                throw new Exception($"Error trying to execute AirportService: {ex.Message}");
            }
        }
    }
}
