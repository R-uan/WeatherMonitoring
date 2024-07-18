namespace WeatherMonitoring.Utilities
{
	public static class RedisKeyGenerator
	{
		public static string GenericKey(string key)
		{
			DateTime date = DateTime.Now;
			string ddMMYYYY = date.ToString("ddMMYYYY");
			return $"{ddMMYYYY}.{key.ToLower()}";
		}
	}
}