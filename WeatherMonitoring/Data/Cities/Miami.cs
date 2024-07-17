using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.DataModels.Cities
{
	public struct Miami : ICity
	{

		public string Name { get; set; } = "Miami";
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public Miami()
		{
			Latitude = 25.783322;
			Longitude = -80.259838;
		}
	}
}