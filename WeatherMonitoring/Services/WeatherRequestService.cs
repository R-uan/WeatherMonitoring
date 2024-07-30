using Serilog;
using System.Net.Http.Json;
using WeatherMonitoring.Data;
using WeatherMonitoring.Utilities;
using WeatherMonitoring.Interfaces;
using WeatherMonitoring.Interfaces.Services;

namespace WeatherMonitoring.Services
{
	public class WeatherRequestServices(AppSettings settings) : IWeatherRequestService
	{
		private readonly AppSettings env = settings;

		public async Task<OpenWeatherResponse> RequestCityWeather(ICity city)
		{
			Log.Information($"{city.Name} hourly weather report requested.");
			string url = $"{env.BaseURL}?lat={city.Latitude}&lon={city.Longitude}&appid={env.OpenWeatherApiKey}";
			using var http = new HttpClient();
			return await http.GetFromJsonAsync<OpenWeatherResponse>(url) ??
			throw new Exception($"Failed to request {city.Name} weather from api");
		}
	}
}