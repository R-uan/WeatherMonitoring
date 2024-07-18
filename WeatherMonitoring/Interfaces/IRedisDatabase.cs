using WeatherMonitoring.Data;

namespace WeatherMonitoring.Interfaces
{
	public interface IRedisDatabase
	{
		Task<string> PushHourlyReport(string key, WeatherReport report);
	}
}
