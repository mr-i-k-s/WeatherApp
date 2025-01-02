namespace WeatherApp.Interfaces
{
	public interface ISettingsService
	{
		string Get(string key);
		bool Set(string key, string value);
		bool Delete(string key);
	}
}
