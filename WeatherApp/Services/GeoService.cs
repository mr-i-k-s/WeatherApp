using Newtonsoft.Json;
using System.Net.Http;
using WeatherApp.Interfaces;
using WeatherApp.MVVM.Models;

namespace WeatherApp.Services
{
	public class GeoService : IGeoService
	{
		public string BaseGeoApiUrl { get; } = "https://api.openweathermap.org/geo/1.0/direct";

		public Task<IGeoData[]> GetGeoDataAsync(string city, string apiKey, int limit = 10)
		{
			IGeoData[] geoData = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(BaseGeoApiUrl);

				var response = client.GetAsync($"?q={city}&limit={limit}&appid={apiKey}").Result;

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = response.Content.ReadAsStringAsync().Result;
					geoData = JsonConvert.DeserializeObject<GeoData[]?>(jsonResponse);
				}
			}

			return Task.FromResult(geoData);
		}
	}
}
