using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Interfaces
{
	public interface IGeoService
	{
		Task<IGeoData[]> GetGeoDataAsync(string city, string apiKey, int limit = 5);
	}
}
