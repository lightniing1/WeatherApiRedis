using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Text.Json;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDistributedCache _distributedCache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult GetWeather()
        {
            var weatherCache= _distributedCache.GetString("weather");

            if (!string.IsNullOrEmpty(weatherCache))
            {
                Console.WriteLine("Hit cache");
                var cacheObject = JsonSerializer.Deserialize<WeatherForecast>(weatherCache);
                return Ok(cacheObject);
            }

            var weather = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(Random.Shared.Next(-20, 55))),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };

            var weatherJson = JsonSerializer.Serialize(weather);
            var memoryCacheEntryOptions = new DistributedCacheEntryOptions  
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60),
                SlidingExpiration = TimeSpan.FromSeconds(15)
            };

            _distributedCache.SetString("weather", weatherJson, memoryCacheEntryOptions);

            Console.WriteLine("Created new object. Added to cache");

            return Ok(weather);
        }
    };      
}