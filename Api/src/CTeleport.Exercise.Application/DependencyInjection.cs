using CTeleport.Exercise.Application.Endpoints.Airports.Queires;
using CTeleport.Exercise.Application.Endpoints.Airports.Response;
using CTeleport.Exercise.Application.Interfaces.UseCase;
using CTeleport.Exercise.Application.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CTeleport.Exercise.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUseCase<AirportsDistanceQuery, AirportDistanceResponse>, AirportsDistanceUseCase>();

        return services;
    }
}
