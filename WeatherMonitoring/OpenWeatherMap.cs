using System.Net.Http.Json;
using Serilog;
using StackExchange.Redis;
using WeatherMonitoring.DataModels;
using WeatherMonitoring.Interfaces;
using WeatherMonitoring.Utilities;

namespace WeatherMonitoring;

public class OpenWeatherMap(IRedisDatabase database, AppSettings settings)
{
	private readonly AppSettings env = settings;
	private readonly IRedisDatabase redis = database;
	public async Task<WeatherReport> RequestWeatherReport(ICity location)
	{
		try
		{
			Log.Information($"{location.Name} hourly weather report requested!");
			string url = $"{env.BaseURL}?lat={location.Latitude}&lon={location.Longitude}&appid={env.OpenWeatherApiKey}";
			using var http = new HttpClient();
			var data = await http.GetFromJsonAsync<OpenWeatherResponse>(url);
			if (data != null)
			{
				Weather weather = new()
				{
					WeatherId = data.Weather[0].Id,
					Main = data.Weather[0].Main,
					Description = data.Weather[0].Description
				};

				WeatherReport weatherReport = new()
				{
					Location = location.Name,
					Humidity = data.Main.Humidity,
					Latitude = data.Coord.Lat,
					Longitude = data.Coord.Lon,
					Pressure = data.Main.Pressure,
					Time = data.Dt,
					WindSpeed = data.Wind.Speed,
					Temp = data.Main.Temp,
					Weather = weather
				};

				var key = await redis.PushHourlyReport(location.Name, weatherReport);
				Log.Information($"{key} created!");
				return weatherReport;
			}
			throw new Exception($"{location.Name} failed. Unable to recover data from API.");
		}
		catch (System.Exception ex)
		{
			Log.Error(ex.Message);
			throw;
		}
	}
}
