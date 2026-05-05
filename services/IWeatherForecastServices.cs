namespace WebApplication1.services
{
    public interface IWeatherForecastServices
    {
        public IEnumerable<WeatherForecast> GetForecasts();
    }
}
