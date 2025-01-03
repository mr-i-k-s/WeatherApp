namespace WeatherApp.Interfaces
{
	public interface IWeatherService
	{
		Task<IWeatherData> GetWeatherDataAsync(string city, string state, string country, string unit, string apiKey);
	}
}
