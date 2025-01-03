using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Interfaces
{
	public interface IWeatherService
	{
		Task<IWeatherData> GetWeatherDataAsync(string city, string state, string country, string unit, string apiKey);
	}
}
