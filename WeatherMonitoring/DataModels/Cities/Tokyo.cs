using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.DataModels.Cities
{
	public struct Tokyo : ICity
	{
		public string Name { get; set; } = "Tokyo";
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public Tokyo()
		{
			Latitude = 35.689487;
			Longitude = 139.691711;
		}
	}
}