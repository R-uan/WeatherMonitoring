using MongoDB.Bson;

namespace WeatherMonitoring.Data.Models
{
	public class DailyAverage
	{
		public ObjectId Id { get; set; }
		public string City { get; set; } = String.Empty;
		public DateTime Date { get; set; }
		public float AvgTemp { get; set; }
		public float AvgHumidity { get; set; }
		public float AvgPressure { get; set; }
		public float AvgWindSpeed { get; set; }
	}
}
