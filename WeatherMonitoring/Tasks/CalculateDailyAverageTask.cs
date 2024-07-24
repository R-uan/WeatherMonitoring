using System.Text.Json;
using Coravel.Invocable;
using Serilog;
using WeatherMonitoring.Data;
using WeatherMonitoring.Data.Models;
using WeatherMonitoring.Data.Mongo;
using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.Tasks
{
	public class CalculateDailyAverageTask(IRedisDatabase redis, DailyAverageDbContext database) : IInvocable
	{
		private readonly IRedisDatabase redis = redis;
		private readonly DailyAverageDbContext db = database;
		public async Task Invoke()
		{
			foreach (var city in TargetCities.All)
			{
				var average = new DailyAverage();
				var day = await redis.YesterdayReports(city.Name);
				if (day != null && day.Count > 0)
				{
					int reportSize = day.Count;

					float avgTemp = 0;
					float avgHumidity = 0;
					float avgPressure = 0;
					float avgWindSpeed = 0;

					for (int i = 0; i < day.Count; i++)
					{
						avgTemp += day[i].Temp;
						avgHumidity += day[i].Humidity;
						avgPressure += day[i].Pressure;
						avgWindSpeed += day[i].WindSpeed;
					}

					average.Date = day[0].Date;
					average.City = day[0].Location;
					average.AvgTemp = avgTemp / reportSize;
					average.AvgPressure = avgPressure / reportSize;
					average.AvgHumidity = avgHumidity / reportSize;
					average.AvgWindSpeed = avgWindSpeed / reportSize;
				}
				await db.AddAsync(average);
				await db.SaveChangesAsync();
			}
		}
	}
}