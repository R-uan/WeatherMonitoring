namespace WeatherMonitoring;

public class WeatherReport
{
	public required string Location { get; set; }
	public long Time { get; set; }
	public float Temp { get; set; }
	public int Humidity { get; set; }
	public int Pressure { get; set; }
	public float WindSpeed { get; set; }
	public float Latitude { get; set; }
	public float Longitude { get; set; }
	public required Weather Weather { get; set; }
}

public class Weather
{
	public int WeatherId { get; set; }
	public required string Main { get; set; }
	public string? Description { get; set; }
}
