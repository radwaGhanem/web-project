using System.Collections;

namespace WebApplication1.services
{
    public class WeatherForcastServices : IWeatherForecastServices
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        public readonly ILogger<WeatherForcastServices> _logger;
        public WeatherForcastServices(ILogger<WeatherForcastServices>logger)
        { 
            _logger=logger;
        
        } 
        public IEnumerable<WeatherForecast> GetForecasts()
        {
            _logger.LogInformation("getting forcast data");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
