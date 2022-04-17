using System.Text.Json.Serialization;

namespace CTeleport.Exercise.MinimalApi.Model
{
    public class AirportInfo
    {
        public string Country { get; set; }

        [JsonPropertyName("city_iata")]
        public string CityIata { get; set; }
        
        public string Iata { get; set; }
        
        public string City { get; set; }
        
        [JsonPropertyName("timezone_region_name")]
        public string TimezoneRegionName { get; set; }
        
        [JsonPropertyName("country_iata")]
        public string CountryIata { get; set; }
        
        public int Rating { get; set; }
        
        public string Name { get; set; }
        
        public Location Location { get; set; }
        
        public string Type { get; set; }
        
        public int Hubs { get; set; }
    }
}