using System.Text.Json.Serialization;

namespace CTeleport.Exercise.MinimalApi.Model
{
    public class AirportDistanceResponse
    {
        [JsonPropertyName("origin_airport")]
        public AirportInfo OriginAirport { get; set; }

        [JsonPropertyName("destiny_airport")]
        public AirportInfo DestinyAirport { get; set; }

        [JsonPropertyName("distance")]
        public string DistanceInKilometers { get; set; }
    }
}
