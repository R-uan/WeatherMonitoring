using Serilog;
using WeatherMonitoring;
using StackExchange.Redis;
using WeatherMonitoring.Utilities;
using Microsoft.Extensions.Configuration;
using WeatherMonitoring.DataModels.Cities;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Coravel;
using Microsoft.Extensions.Hosting;
using WeatherMonitoring.Tasks;

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
builder.Services.AddSingleton<AppSettings>(env);
builder.Services.AddScoped<WeatherReportChannel>();
builder.Services.AddScoped<HourlyWeatherRequestTask>();
builder.Services.AddSingleton<IDatabase>(redisDatabase);
builder.Services.AddScoped<IRedisDatabase, RedisDatabase>();
builder.Services.AddSingleton<ConnectionFactory>(rabbitFactory);
builder.Services.AddSerilog((services, lc) => lc.ReadFrom.Configuration(configuration).ReadFrom.Services(services));

var app = builder.Build();

/* app.Services.UseScheduler(scheduler =>
{
	scheduler.Schedule<HourlyWeatherRequest>().EveryFiveSeconds();
});
 */

await app.Services.GetService<HourlyWeatherRequestTask>()!.Invoke();

/* var messageSender = app.Services.GetService<RabbitMessageQueue>()!; */
/* await messageSender.SendMessage(); */

app.Run();