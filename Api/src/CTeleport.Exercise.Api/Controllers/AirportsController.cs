using System.Diagnostics.CodeAnalysis;
using CTeleport.Exercise.Application.Endpoints.Airports.Queires;
using CTeleport.Exercise.Application.Endpoints.Airports.Response;
using CTeleport.Exercise.Application.Interfaces.UseCase;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CTeleport.Exercise.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/airports")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        private readonly IUseCase<AirportsDistanceQuery, AirportDistanceResponse> _useCase;
        private readonly ILogger<AirportsController> _logger;
        public AirportsController(IUseCase<AirportsDistanceQuery, AirportDistanceResponse> useCase, ILogger<AirportsController> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        // GET: api/<AirportsController>
        [HttpGet]
        [Route("distance")]
        [ProducesResponseType(typeof(AirportDistanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAirpotsDistance([FromQuery] AirportsDistanceQuery airportsDistanceQuery)
        {
            try
            {
                return Ok(await _useCase.ExecuteAsync(airportsDistanceQuery));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error executing Airports distance API: {e.Message}");
                return Problem(detail: e.Message);
            }
        }
    }
}
