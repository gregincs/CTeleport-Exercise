using CTeleport.Exercise.MinimalApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var baseAddress = builder.Configuration.GetValue<string>("AirportInfoBaseAddress");
var stage = builder.Configuration.GetValue<string>("AirportInfoStage");
var jsonConfig =  new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

app.MapGet("/airports/distance", async ([FromQuery] string origin, [FromQuery] string destiny, [FromServices] IHttpClientFactory httpClientFactory) =>
{
    try
    {

        if (origin.Length != 3 || destiny.Length != 3)
        {
            return Results.BadRequest("Origin and Destiny length must be 3 characters long");
        }
        var query = new AirportsDistanceQuery 
        {
            Origin = origin.ToUpper(),
            Destiny = destiny.ToUpper(),
        };

        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(baseAddress);

        var responseOriginTask = httpClient.GetAsync($"{stage}/{query.Origin}");
        var responseDestinyTask = httpClient.GetAsync($"{stage}/{query.Destiny}");

        var airportOriginInfoResponse = await responseOriginTask;
        var airportDestinyInfoResponse = await responseDestinyTask;
        airportOriginInfoResponse.EnsureSuccessStatusCode();
        airportDestinyInfoResponse.EnsureSuccessStatusCode();

        var airportOriginInfo = JsonSerializer.Deserialize<AirportInfo>(await airportOriginInfoResponse.Content.ReadAsStringAsync(), jsonConfig);
        var airportDestinyInfo = JsonSerializer.Deserialize<AirportInfo>(await airportDestinyInfoResponse.Content.ReadAsStringAsync(), jsonConfig);

        var airportOriginCoordinate = new GeoCoordinatePortable.GeoCoordinate(airportOriginInfo.Location.Lat, airportOriginInfo.Location.Lon);
        var airportDestinyCoordinate = new GeoCoordinatePortable.GeoCoordinate(airportDestinyInfo.Location.Lat, airportDestinyInfo.Location.Lon);

        var distanceInKilometers = Math.Round(airportOriginCoordinate.GetDistanceTo(airportDestinyCoordinate) / 1000, 2);

        var result = new AirportDistanceResponse 
        {
            DestinyAirport = airportDestinyInfo,
            OriginAirport = airportOriginInfo,
            DistanceInKilometers = $"{distanceInKilometers} Km"
        };

        return Results.Ok(result);
    }
    catch (global::System.Exception e)
    {
        return Results.Problem(e.Message);
    }
});

app.Run();