using Serilog;
using Coravel.Invocable;
using System.Net.Http.Json;
using WeatherMonitoring.Data;
using WeatherMonitoring.Rabbit;
using WeatherMonitoring.Utilities;
using WeatherMonitoring.Interfaces;
using WeatherMonitoring.Data.Models;

namespace WeatherMonitoring.Tasks
{
	public class HourlyWeatherRequestTask(IRedisDatabase database, AppSettings settings, WeatherReportChannel channel) : IInvocable
	{
		private readonly AppSettings env = settings;
		private readonly IRedisDatabase redis = database;
		private readonly WeatherReportChannel reportChannel = channel;

		public async Task Invoke()
		{
			Log.Information($"Requesting hourly weather report . . .");
			List<WeatherReport> reports = [];
			foreach (var city in TargetCities.All)
			{
				Log.Information($"{city.Name} hourly weather report requested.");
				string url = $"{env.BaseURL}?lat={city.Latitude}&lon={city.Longitude}&appid={env.OpenWeatherApiKey}";
				using var http = new HttpClient();
				var data = await http.GetFromJsonAsync<OpenWeatherResponse>(url) ??
				throw new Exception($"Failed to request {city.Name} weather from api");

				Weather weather = new()
				{
					WeatherId = data.Weather[0].Id,
					Main = data.Weather[0].Main,
					Description = data.Weather[0].Description
				};

				WeatherReport weatherReport = new()
				{
					Date = DateTime.Now,
					Location = city.Name,
					Humidity = data.Main.Humidity,
					Latitude = data.Coord.Lat,
					Longitude = data.Coord.Lon,
					Pressure = data.Main.Pressure,
					Time = data.Dt,
					WindSpeed = data.Wind.Speed,
					Temp = data.Main.Temp,
					Weather = weather
				};

				var key = await redis.PushHourlyReport(city.Name, weatherReport);
				Log.Information($"{city.Name} report created - {key}");
				reports.Add(weatherReport);
			}
			await reportChannel.SendHourlyReport(reports);
			await Task.CompletedTask;
		}
	}
}