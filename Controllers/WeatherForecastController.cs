using Microsoft.AspNetCore.Mvc;
using WebApplication1.services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/weather")]

    
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastServices _services;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastServices services) 
        {
            _logger= logger;    
            _services= services;
        }
        

        [HttpGet("getdata")]
        public IEnumerable<WeatherForecast> Get()
        {
          return _services.GetForecasts();
        }
    }
}
