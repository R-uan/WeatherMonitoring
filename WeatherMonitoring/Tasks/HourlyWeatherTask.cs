using Coravel.Invocable;
using Serilog;
using WeatherMonitoring.DataModels.Cities;
using WeatherMonitoring.Interfaces;

namespace WeatherMonitoring.Tasks
{
	public class HourlyWeatherTask(OpenWeatherMap request, WeatherReportChannel channel) : IInvocable
	{
		private readonly OpenWeatherMap weather = request;
		private readonly WeatherReportChannel reportChannel = channel;

		public async Task Invoke()
		{
			Log.Information($"Hourly weather report task triggered.");
			List<ICity> targetCities = [new Miami(), new Salvador(), new Sydney(), new Shibuya(), new London()];
			List<WeatherReport> reports = [];
			foreach (var city in targetCities)
			{
				var report = await weather.RequestWeatherReport(city);
				reports.Add(report);
			}

			await reportChannel.SendHourlyReport(reports);
			await Task.CompletedTask;
		}
	}
}