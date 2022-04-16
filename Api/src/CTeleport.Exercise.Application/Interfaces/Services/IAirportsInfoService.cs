using CTeleport.Exercise.Domain.Entities;

namespace CTeleport.Exercise.Application.Interfaces.Services
{
    public interface IAirportsInfoService
    {
        Task<AirportInfo> GetAirportInfoAsync(string airportIataCode);
    }
}
