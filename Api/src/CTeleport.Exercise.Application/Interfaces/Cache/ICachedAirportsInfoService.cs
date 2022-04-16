using CTeleport.Exercise.Domain.Entities;

namespace CTeleport.Exercise.Application.Interfaces.Cache
{
    public interface ICachedAirportsInfoService
    {
        Task<AirportInfo> GetCachedAirportInfo(string airport);
    }
}
