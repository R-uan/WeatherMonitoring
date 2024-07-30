using Discord.WebSocket;
using WeatherMonitoring.Tasks;

namespace WeatherMonitoring.Discord
{
	public class DiscordMessageHandler
	{
		private readonly HourlyWeatherRequestTask _hourlyWeatherRequest;
		public DiscordMessageHandler(HourlyWeatherRequestTask task)
		{
			_hourlyWeatherRequest = task;
		}

		public async Task HandleMessageEvent(SocketMessage message)
		{
			if (!message.Content.StartsWith("sudo")) return;
			String[] command = message.Content.Split(" ");
			if (command.Length == 1) await message.Channel.SendMessageAsync("Missing command. Try `sudo help` for instructions.");
		}
	}
}