using System.Text.Json.Serialization;
using CTeleport.Exercise.Domain.Entities;

namespace CTeleport.Exercise.Application.Endpoints.Airports.Response
{
    public record AirportDistanceResponse
    {
        [JsonPropertyName("origin_airport")]
        public AirportInfo OriginAirport { get; set; }

        [JsonPropertyName("destiny_airport")]
        public AirportInfo DestinyAirport { get; set; }

        [JsonPropertyName("distance")]
        public string DistanceInKilometers { get; set; }
    }
}
