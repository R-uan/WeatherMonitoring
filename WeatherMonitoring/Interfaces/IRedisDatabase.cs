using WeatherMonitoring.Data.Models;

namespace WeatherMonitoring.Interfaces
{
	public interface IRedisDatabase
	{
		Task<string> PushHourlyReport(string key, WeatherReport report);
		Task<List<WeatherReport>> YesterdayReports(string city);
	}
}
