using System.Runtime.Serialization;

namespace WeatherMonitoring.Utilities
{
	public static class RedisKeyGenerator
	{
		public static string GenericKey(string key)
		{
			DateTime date = DateTime.Now;
			string ddMM = date.ToString("ddMM");
			return $"{ddMM}{key}Report";
		}
	}
}