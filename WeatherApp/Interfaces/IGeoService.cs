namespace WeatherApp.Interfaces
{
	public interface IGeoService
	{
		Task<IGeoData[]> GetGeoDataAsync(string city, string apiKey, int limit = 5);
	}
}
