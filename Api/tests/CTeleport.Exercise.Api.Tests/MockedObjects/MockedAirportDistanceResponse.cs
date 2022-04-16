using System.Text.Json;
using CTeleport.Exercise.Application.Endpoints.Airports.Response;

namespace CTeleport.Exercise.Api.Tests.MockedObjects
{
    public class MockedAirportDistanceResponse
    {
        private static readonly string _responseJson = "{\"origin_airport\":{\"country\":\"Brazil\",\"city_iata\":\"SAO\",\"iata\":\"GRU\",\"city\":\"Sao Paulo\",\"timezone_region_name\":\"America/Sao_Paulo\",\"country_iata\":\"BR\",\"rating\":3,\"name\":\"Guarulhos International\",\"location\":{\"lat\":-23.425668,\"lon\":-46.481926},\"type\":\"airport\",\"hubs\":1},\"destiny_airport\":{\"country\":\"Brazil\",\"city_iata\":\"CWB\",\"iata\":\"CWB\",\"city\":\"Curitiba\",\"timezone_region_name\":\"America/Sao_Paulo\",\"country_iata\":\"BR\",\"rating\":1,\"name\":\"Afonso Pena International\",\"location\":{\"lat\":-25.535763,\"lon\":-49.173298},\"type\":\"airport\",\"hubs\":0},\"distance\":\"359,78 Km\"}";
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static AirportDistanceResponse GetResponseOk()
        {
            var response = JsonSerializer.Deserialize<AirportDistanceResponse>(_responseJson, _options);

            return response;
        }
    }
}
