using CTeleport.Exercise.Domain.Entities;
using Refit;

namespace CTeleport.Exercise.Infrastructure.Services.HttpClients
{
    public interface IAirportInfoClient
    {
        [Get("/airports/{iataCode}")]
        public Task<AirportInfo> GetAirportInfoByIataCodeAsync(string iataCode);
    }
}
