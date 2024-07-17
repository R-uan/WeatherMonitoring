using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.DataModels.Cities
{
	public struct Shibuya : ICity
	{
		public string Name { get; set; } = "Shibuya";
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public Shibuya()
		{
			Latitude = 35.689487;
			Longitude = 139.691711;
		}
	}
}