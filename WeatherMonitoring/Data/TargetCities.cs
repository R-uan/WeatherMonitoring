using WeatherMonitoring.Data.Models;
using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.Data
{
	public static class TargetCities
	{
		public static readonly List<ICity> All =
		[
			new City()
		{
			Name = "Salvador",
			Latitude = -12.977749,
			Longitude = -38.501629
		},
		new City()
		{
			Name = "London",
			Latitude = 51.507351,
			Longitude = -0.127758
		},
		new City()
		{
			Name = "Miami",
			Latitude = 25.783322,
			Longitude = -80.259838
		},
		new City()
		{
			Name = "Sydney",
			Latitude = -33.868820,
			Longitude = 151.209290
		},
		new City()
		{
			Name = "Shibuya",
			Latitude = 35.689487,
			Longitude = 139.691711
		},
	];
	}
}