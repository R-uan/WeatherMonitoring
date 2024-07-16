using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.DataModels.Cities
{
	public struct Salvador : ICity
	{
		public string Name { get; set; } = "Salvador";
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public Salvador()
		{
			Latitude = -12.977749;
			Longitude = -38.501629;
		}
	}
}