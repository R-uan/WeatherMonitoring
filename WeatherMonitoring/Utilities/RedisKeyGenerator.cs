namespace WeatherMonitoring.Utilities
{
	public static class RedisKeyGenerator
	{
		//
		//	Summary:
		//		Generates a key using a string provided.
		public static string GenericKey(string key)
		{
			DateTime date = DateTime.Now;
			string ddMMyyyy = date.ToString("ddMMyyyy");
			return $"{ddMMyyyy}.{key.ToLower()}";
		}

		public static string GenericKey(string key, DateTime date)
		{
			string ddMMyyyy = date.ToString("ddMMyyyy");
			return $"{ddMMyyyy}.{key.ToLower()}";
		}
	}
}