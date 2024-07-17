using System.Text.Json.Serialization;

namespace WeatherMonitoring;

public class WeatherReport
{
	[JsonPropertyName("location")]
	public required string Location { get; set; }
	[JsonPropertyName("time")]
	public long Time { get; set; }
	[JsonPropertyName("temp")]
	public float Temp { get; set; }
	[JsonPropertyName("humidity")]
	public int Humidity { get; set; }
	[JsonPropertyName("pressure")]
	public int Pressure { get; set; }
	[JsonPropertyName("windSpeed")]
	public float WindSpeed { get; set; }
	[JsonPropertyName("latitude")]
	public float Latitude { get; set; }
	[JsonPropertyName("longitude")]
	public float Longitude { get; set; }
	[JsonPropertyName("weather")]
	public required Weather Weather { get; set; }
}

public class Weather
{
	[JsonPropertyName("weatherId")]
	public int WeatherId { get; set; }
	[JsonPropertyName("main")]
	public required string Main { get; set; }
	[JsonPropertyName("description")]
	public string? Description { get; set; }
}
