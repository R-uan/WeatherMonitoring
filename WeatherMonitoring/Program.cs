using Serilog;
using Coravel;
using MongoDB.Driver;
using RabbitMQ.Client;
using StackExchange.Redis;
using WeatherMonitoring.Data;
using WeatherMonitoring.Tasks;
using WeatherMonitoring.Utilities;
using WeatherMonitoring.Interfaces;
using Microsoft.Extensions.Hosting;
using WeatherMonitoring.Data.Mongo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using WeatherMonitoring.Discord;
using WeatherMonitoring.Services;

var builder = Host.CreateApplicationBuilder();

var env = new AppSettings();
var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
configuration.GetSection("AppSettings").Bind(env);

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Debug("Starting the application . . .");

#region Discord 

var discord = new DiscordSocketClient(new DiscordSocketConfig() { GatewayIntents = Discord.GatewayIntents.All });
await discord.LoginAsync(Discord.TokenType.Bot, env.DiscordToken);

discord.Ready += HandleReadyEvent;

Task HandleReadyEvent()
{
	Log.Debug($"{discord.CurrentUser.Username} is now connected!");
	discord.GetGuild(1052664028087992402)
	.GetTextChannel(1257102853772808294)
	.SendMessageAsync("Hello");
	return Task.CompletedTask;
}

#endregion

#region Database connection 

var redisConnection = await ConnectionMultiplexer.ConnectAsync("localhost");
var redisDatabase = redisConnection.GetDatabase();
var rabbitFactory = new ConnectionFactory { HostName = "localhost", UserName = "user", Password = "password" };
var mongoClient = new MongoClient("mongodb://dev:dev@localhost:27017");

#endregion

#region Services 

builder.Services.AddScheduler();
builder.Services.AddDbContext<DailyAverageDbContext>(a =>
{
	a.UseMongoDB(mongoClient, "WeatherMonitoring");
});
builder.Services.AddSerilog((services, lc) => lc.ReadFrom.Configuration(configuration).ReadFrom.Services(services));
builder.Services.AddScoped<RabbitChannelService>();
builder.Services.AddScoped<HourlyWeatherRequestTask>();
builder.Services.AddScoped<CalculateDailyAverageTask>();
builder.Services.AddScoped<IRedisDatabase, RedisDatabase>();

builder.Services.AddSingleton<AppSettings>(env);
builder.Services.AddSingleton<DiscordMessageHandler>();
builder.Services.AddSingleton<IDatabase>(redisDatabase);
builder.Services.AddSingleton<ConnectionFactory>(rabbitFactory);

#endregion

var app = builder.Build();

app.Services.UseScheduler(scheduler =>
{
	scheduler.Schedule<HourlyWeatherRequestTask>().HourlyAt(30);
	scheduler.Schedule<CalculateDailyAverageTask>().DailyAtHour(0);
});

discord.MessageReceived += app.Services.GetService<DiscordMessageHandler>()!.HandleMessageEvent;

await discord.StartAsync();
app.Run();