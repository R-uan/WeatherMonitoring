using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.DataModels.Cities
{
	public struct London : ICity
	{
		public string Name { get; set; } = "London";
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public London()
		{
			Latitude = 51.507351;
			Longitude = -0.127758;
		}
	}
}