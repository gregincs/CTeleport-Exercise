using System.Text.Json;
using CTeleport.Exercise.Domain.Entities;

namespace CTeleport.Exercise.Infrastructure.Tests.MockedObjects
{
    public class MockedAirportInfo
    {
        private static readonly string _gruJson = "{\"country\":\"Brazil\",\"city_iata\":\"SAO\",\"iata\":\"GRU\",\"city\":\"Sao Paulo\",\"timezone_region_name\":\"America/Sao_Paulo\",\"country_iata\":\"BR\",\"rating\":3,\"name\":\"Guarulhos International\",\"location\":{\"lon\":-46.481926,\"lat\":-23.425668},\"type\":\"airport\",\"hubs\":1}";
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static AirportInfo GetGruOk()
        {
            var airportInfo = JsonSerializer.Deserialize<AirportInfo>(_gruJson, _options);

            return airportInfo;
        }
    }
}
