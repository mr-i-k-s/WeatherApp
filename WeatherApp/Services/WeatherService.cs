using Newtonsoft.Json;
using System.Net.Http;
using System.Net.NetworkInformation;
using WeatherApp.Interfaces;
using WeatherApp.MVVM.Models;

namespace WeatherApp.Services
{
	public class WeatherService : IWeatherService
	{
		public string BaseWeatherApiUrl { get; } = "https://api.openweathermap.org/data/2.5/weather";

		public Task<IWeatherData> GetWeatherDataAsync(string city, string state, string country, string unit, string apiKey)
		{
			IWeatherData weatherData = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseWeatherApiUrl);

                var response = httpClient.GetAsync($"?q={city ?? ""},{state ?? ""},{country ?? ""}&appid={apiKey}&units={unit}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    weatherData = JsonConvert.DeserializeObject<WeatherData?>(jsonResponse);
                }
            }

			return Task.FromResult(weatherData);
		}
	}
}
