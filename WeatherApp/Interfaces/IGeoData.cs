using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Interfaces
{
	public interface IGeoData
	{
		public string Name { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
	}
}
