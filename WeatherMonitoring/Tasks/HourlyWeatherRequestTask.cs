using Serilog;
using Coravel.Invocable;
using WeatherMonitoring.Data;
using WeatherMonitoring.Interfaces;
using WeatherMonitoring.Data.Models;
using WeatherMonitoring.Interfaces.Services;

namespace WeatherMonitoring.Tasks
{
	public class HourlyWeatherRequestTask(IRedisDatabase database, IRabbitChannelService rabbitChannel, IWeatherRequestService weatherService) : IInvocable
	{
		public bool Activated { get; set; } = true;
		private readonly IRedisDatabase redis = database;
		private readonly IRabbitChannelService _rabbitChannel = rabbitChannel;
		private readonly IWeatherRequestService _weatherService = weatherService;

		public async Task Invoke()
		{
			if (!Activated) return;
			try
			{
				Log.Information($"Requesting hourly weather report . . .");
				List<WeatherReport> reports = [];

				foreach (var city in TargetCities.All)
				{
					var data = await _weatherService.RequestCityWeather(city);

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

				await _rabbitChannel.SendHourlyReport(reports);
			}
			catch (System.Exception ex)
			{
				Log.Error(ex.Message);
			}
		}
	}
}