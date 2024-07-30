using Serilog;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using WeatherMonitoring.Data.Models;
using WeatherMonitoring.Interfaces.Services;

namespace WeatherMonitoring.Services
{
	public class RabbitChannelService(ConnectionFactory factory) : IRabbitChannelService
	{
		private readonly ConnectionFactory rabbitFactory = factory;

		public async Task SendHourlyReport(List<WeatherReport> reports)
		{
			Log.Information("Sending hourly report through 'hourly-report' rabbit channel.");
			using var connection = rabbitFactory.CreateConnection();
			using var channel = connection.CreateModel();
			channel.QueueDeclare(queue: "hourly-report", durable: false, exclusive: false, autoDelete: false, arguments: null);

			var reportSerialized = JsonSerializer.Serialize(reports);
			var payload = Encoding.UTF8.GetBytes(reportSerialized);
			channel.BasicPublish(exchange: "", routingKey: "hourly-report", basicProperties: null, body: payload);
			await Task.CompletedTask;
		}
	}
}
