using WeatherMonitoring.Data;
using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.Data.Models
{
	public class City : ICity
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required double Latitude { get; set; }
		public required double Longitude { get; set; }
		public List<DailyAverage>? DailyAverage { get; set; }
	}
}