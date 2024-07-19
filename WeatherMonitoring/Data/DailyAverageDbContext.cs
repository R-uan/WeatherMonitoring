using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Data.Models;
using MongoDB.EntityFrameworkCore.Extensions;

namespace WeatherMonitoring.Data.Mongo
{
	public class DailyAverageDbContext(DbContextOptions<DailyAverageDbContext> options) : DbContext(options)
	{
		public DbSet<DailyAverageDbContext> DailyAverage { get; init; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<DailyAverage>().ToCollection("daily_averages");
		}
	}
}