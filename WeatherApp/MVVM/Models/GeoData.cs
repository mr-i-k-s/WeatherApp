using WeatherApp.Interfaces;

namespace WeatherApp.MVVM.Models
{
	public class GeoData : IGeoData
	{
		public string Name { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
	}
}
