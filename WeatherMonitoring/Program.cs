using Serilog;
using Coravel;
using MongoDB.Driver;
using RabbitMQ.Client;
using StackExchange.Redis;
using WeatherMonitoring.Data;
using WeatherMonitoring.Tasks;
using WeatherMonitoring.Rabbit;
using WeatherMonitoring.Utilities;
using WeatherMonitoring.Interfaces;
using Microsoft.Extensions.Hosting;
using WeatherMonitoring.Data.Mongo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder();

Console.WriteLine("Hello there");
var env = new AppSettings();
var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
configuration.GetSection("AppSettings").Bind(env);
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Debug("Starting the application . . .");

var redisConnection = await ConnectionMultiplexer.ConnectAsync("localhost");
var redisDatabase = redisConnection.GetDatabase();
var rabbitFactory = new ConnectionFactory { HostName = "localhost", UserName = "user", Password = "password" };

var mongoClient = new MongoClient("mongodb://dev:dev@localhost:27017");

builder.Services.AddScheduler();
builder.Services.AddDbContext<DailyAverageDbContext>(a =>
{
	a.UseMongoDB(mongoClient, "WeatherMonitoring");
});
builder.Services.AddSerilog((services, lc) => lc.ReadFrom.Configuration(configuration).ReadFrom.Services(services));

builder.Services.AddScoped<WeatherReportChannel>();
builder.Services.AddScoped<HourlyWeatherRequestTask>();
builder.Services.AddScoped<CalculateDailyAverageTask>();
builder.Services.AddScoped<IRedisDatabase, RedisDatabase>();

builder.Services.AddSingleton<AppSettings>(env);
builder.Services.AddSingleton<IDatabase>(redisDatabase);
builder.Services.AddSingleton<ConnectionFactory>(rabbitFactory);

var app = builder.Build();
app.Services.UseScheduler(scheduler =>
{
	scheduler.Schedule<HourlyWeatherRequestTask>().HourlyAt(30);
	scheduler.Schedule<CalculateDailyAverageTask>().DailyAtHour(0);
});

app.Run();