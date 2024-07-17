using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Serilog;
using StackExchange.Redis;
using WeatherMonitoring.Utilities;

namespace WeatherMonitoring;

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
}
