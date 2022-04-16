using System.Text.RegularExpressions;
using CTeleport.Exercise.Application.Interfaces.Cache;
using CTeleport.Exercise.Application.Interfaces.Services;
using CTeleport.Exercise.Infrastructure.Configuration;
using CTeleport.Exercise.Infrastructure.Services;
using CTeleport.Exercise.Infrastructure.Services.Cached;
using CTeleport.Exercise.Infrastructure.Services.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Refit;

namespace CTeleport.Exercise.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ICachedAirportsInfoService, CachedAirportsInfoService>();
        services.AddTransient<IAirportsInfoService, AirportsInfoService>();

        var policyRetryConfig = configuration.GetSection("PolicyRetryConfig").Get<PolicyRetryConfig>();

        AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(policyRetryConfig.Retry, _ => TimeSpan.FromMilliseconds(policyRetryConfig.Wait));

        AsyncTimeoutPolicy<HttpResponseMessage> timeoutPolicy = Policy
            .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(policyRetryConfig.Timeout));

        services.AddRefitClient<IAirportInfoClient>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(configuration["AirportInfoBaseAddress"]))
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        services.AddMemoryCache();

        return services;
    }
}
