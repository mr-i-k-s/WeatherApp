using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Interfaces
{
	public interface ISettingsService
	{
		string Get(string key);
		bool Set(string key, string value);
		bool Delete(string key);
	}
}
