using Vertical.Slice.WebApi.Entities;

namespace Vertical.Slice.WebApi.Database
{
    public interface IWeatherForecastRepository
    {
        Task<WeatherForecast[]> GetForecasts();
    }
}
