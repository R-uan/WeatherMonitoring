namespace WeatherMonitoring;

public interface IRedisDatabase
{
	Task<string> PushHourlyReport(string key, WeatherReport report);
}
