using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.DataModels.Cities
{
	public class Sydney : ICity
	{
		public string Name { get; set; } = "Sydney";

		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public Sydney()
		{
			Latitude = -33.868820;
			Longitude = 151.209290;
		}
	}
}