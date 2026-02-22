using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                ))
                .ToArray();
        }

        [HttpPost(Name = "CreateWeatherForecast")]
        public IActionResult CreateWeatherForecast([FromBody] WeatherForecast weatherForecast)
        {
            // In a real application, you would typically save the weatherForecast to a database here.

            // For demonstration purposes, we'll just return the created weather forecast with a 201 status code.
            return CreatedAtAction(nameof(Get), new { date = weatherForecast.Date }, weatherForecast);
        }
        
    }
}
