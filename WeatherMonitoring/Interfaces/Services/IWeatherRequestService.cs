using WeatherMonitoring.Data;
using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.Interfaces.Services
{
	public interface IWeatherRequestService
	{
		Task<OpenWeatherResponse> RequestCityWeather(ICity city);
	}
}
