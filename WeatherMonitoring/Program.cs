using Serilog;
using Coravel;
using RabbitMQ.Client;
using StackExchange.Redis;
using WeatherMonitoring.Tasks;
using WeatherMonitoring.Rabbit;
using WeatherMonitoring.Utilities;
using WeatherMonitoring.Interfaces;
using Microsoft.Extensions.Hosting;
using WeatherMonitoring.Data.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder();

var env = new AppSettings();
var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
configuration.GetSection("AppSettings").Bind(env);

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Debug("Starting the application . . .");

var redisConnection = await ConnectionMultiplexer.ConnectAsync("localhost");
var redisDatabase = redisConnection.GetDatabase();
var rabbitFactory = new ConnectionFactory { HostName = "localhost", UserName = "user", Password = "password" };

builder.Services.AddScheduler();
builder.Services.AddSerilog((services, lc) => lc.ReadFrom.Configuration(configuration).ReadFrom.Services(services));

builder.Services.AddScoped<WeatherReportChannel>();
builder.Services.AddScoped<HourlyWeatherRequestTask>();
builder.Services.AddScoped<IRedisDatabase, RedisDatabase>();

builder.Services.AddSingleton<AppSettings>(env);
builder.Services.AddSingleton<IDatabase>(redisDatabase);
builder.Services.AddSingleton<ConnectionFactory>(rabbitFactory);

var app = builder.Build();

app.Services.UseScheduler(scheduler =>
{
	scheduler.Schedule<HourlyWeatherRequestTask>().HourlyAt(30);
});

app.Run();