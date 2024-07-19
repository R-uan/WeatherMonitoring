using Serilog;
using System.Text.Json;
using StackExchange.Redis;
using WeatherMonitoring.Utilities;
using WeatherMonitoring.Interfaces;
using WeatherMonitoring.Data.Models;

namespace WeatherMonitoring.Data
{
	public class RedisDatabase(IDatabase database) : IRedisDatabase
	{
		private readonly IDatabase redis = database;
		public async Task<string> PushHourlyReport(string city, WeatherReport report)
		{
			try
			{
				var key = RedisKeyGenerator.GenericKey(city);
				string serialize = JsonSerializer.Serialize(report);
				RedisValue redisReport = serialize;
				TimeSpan ttl = TimeSpan.FromHours(25);
				await redis.ListRightPushAsync(key, redisReport);
				await redis.KeyExpireAsync(key, ttl);
				return key;
			}
			catch (System.Exception ex)
			{
				Log.Error(ex.Message, ex);
				throw;
			}
		}

		public async Task<List<WeatherReport>> YesterdayReports(string city)
		{
			try
			{
				List<WeatherReport> reports = [];
				DateTime yesterday = DateTime.Now.AddDays(-1);
				var key = RedisKeyGenerator.GenericKey(city, yesterday);
				Log.Debug(key);
				var list = await redis.ListRangeAsync(key);
				foreach (var report in list)
				{
					var parse = JsonSerializer.Deserialize<WeatherReport>(report!);
					if (parse != null) reports.Add(parse);
				}
				return reports;
			}
			catch (System.Exception ex)
			{
				Log.Error(ex.Message);
				throw;
			}
		}
	}
}
