using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Interfaces
{
	public interface ISysInfo
	{
		public double Sunrise { get; set; }
		public double Sunset { get; set; }
	}
}
