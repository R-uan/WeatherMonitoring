namespace WeatherMonitoring.Interfaces
{
	public interface ICity
	{
		string Name { get; set; }
		double Latitude { get; set; }
		double Longitude { get; set; }
	}
}
