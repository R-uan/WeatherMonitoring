namespace WeatherMonitoring;

public class WeatherReportMessage
{
	public string date;
	public string weatherReports;

	public WeatherReportMessage(string reports)
	{
		this.weatherReports = reports;
		this.date = DateTime.Now.ToString();
	}
}
