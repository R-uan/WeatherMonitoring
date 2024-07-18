using System.Text.Json.Serialization;

namespace WeatherMonitoring.Data
{
	public class OpenWeatherResponse
	{
		public int Id { get; set; }
		public long Dt { get; set; }
		public int Cod { get; set; }
		public int Timezone { get; set; }
		public int Visibility { get; set; }
		public required Sys Sys { get; set; }
		public required Main Main { get; set; }
		public required Wind Wind { get; set; }
		public required string Base { get; set; }
		public required string Name { get; set; }
		public required Coord Coord { get; set; }
		public required Clouds Clouds { get; set; }
		public required List<OWeather> Weather { get; set; }
	}

	public class Coord
	{
		public float Lon { get; set; }
		public float Lat { get; set; }
	}
	public class OWeather
	{
		public int Id { get; set; }
		public required string Main { get; set; }
		public required string Description { get; set; }
		public required string Icon { get; set; }
	}

	public class Main
	{
		public float Temp { get; set; }
		public float FeelsLike { get; set; }
		public float TempMin { get; set; }
		public float TempMax { get; set; }
		public int Pressure { get; set; }
		public int Humidity { get; set; }
		public int SeaLevel { get; set; }
		public int GrndLevel { get; set; }
	}

	public class Wind
	{
		public float Speed { get; set; }
		public int Deg { get; set; }
		public float Gust { get; set; }
	}

	public class Clouds
	{
		public int All { get; set; }
	}

	public class Sys
	{
		public int Type { get; set; }
		public int Id { get; set; }
		public required string Country { get; set; }
		public long Sunrise { get; set; }
		public long Sunset { get; set; }
	}

}