using WeatherMonitoring.Data.Models;

namespace WeatherMonitoring.Interfaces.Services
{
	public interface IRabbitChannelService
	{
		Task SendHourlyReport(List<WeatherReport> reports);
	}
}